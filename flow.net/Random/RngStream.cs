using System;
using System.Xml.Serialization;
using FLOW.NET.Operational;

namespace FLOW.NET.Random
{
    public class RngStream
    {
        private static double norm = 2.328306549295727688e-10;
        private static double m1 = 4294967087.0;
        private static double m2 = 4294944443.0;
        private static double a12 = 1403580.0;
        private static double a13n = 810728.0;
        private static double a21 = 527612.0;
        private static double a23n = 1370589.0;
        private static double two17 = 131072.0;
        private static double two53 = 9007199254740992.0;
        private static double invtwo24 = 5.9604644775390625e-8;

        private static double[,] InvA1 = 
		{
		{ 184888585.0, 0.0, 1945170933.0 },
		{         1.0, 0.0,          0.0 },
		{         0.0, 1.0,          0.0 }
		};
        private static double[,] InvA2 = 
		{
		{ 0.0, 360363334.0, 4225571728.0 },
		{ 1.0,         0.0,          0.0 },
		{ 0.0,         1.0,          0.0 }
		};
        private static double[,] A1p0 =  
		{
		{       0.0,       1.0,      0.0 },
		{       0.0,       0.0,      1.0 },
		{ -810728.0, 1403580.0,      0.0 }
		};
        private static double[,] A2p0 =  
		{
		{        0.0,   1.0,         0.0 },
		{        0.0,   0.0,         1.0 },
		{ -1370589.0,   0.0,    527612.0 }
		};
        private static double[,] A1p76 = 
		{
		{      82758667.0, 1871391091.0, 4127413238.0 },
		{    3672831523.0,   69195019.0, 1871391091.0 },
		{    3672091415.0, 3528743235.0,   69195019.0 }
		};
        private static double[,] A2p76 = 
		{
		{    1511326704.0, 3759209742.0, 1610795712.0 },
		{    4292754251.0, 1511326704.0, 3889917532.0 },
		{    3859662829.0, 4292754251.0, 3708466080.0 }
		};
        private static double[,] A1p127 = 
		{
		{    2427906178.0, 3580155704.0,  949770784.0 },
		{     226153695.0, 1230515664.0, 3580155704.0 },
		{    1988835001.0,  986791581.0, 1230515664.0 }
		};
        private static double[,] A2p127 = 
		{
		{    1464411153.0,  277697599.0, 1610723613.0 },
		{      32183930.0, 1464411153.0, 1022607788.0 },
		{    2824425944.0,   32183930.0, 2093834863.0 }
		};

        private static double[] nextSeed = { 12345, 12345, 12345, 12345, 12345, 12345 };

        private double[] cg = new double[6];
        private double[] bg = new double[6];
        private double[] ig = new double[6];

        public double[] Cg
        {
            get { return this.cg; }
            set { this.cg = value; }
        }

        private bool anti;
        private bool prec53;

        private static double multModM(double a, double s, double c, double m)
        {
            double v;
            int a1;
            v = a * s + c;
            if (v >= two53 || v <= -two53)
            {
                a1 = (int)(a / two17);
                a -= a1 * two17;
                v = a1 * s;
                a1 = (int)(v / m);
                v -= a1 * m;
                v = v * two17 + a * s + c;
            }
            a1 = (int)(v / m);
            if ((v -= a1 * m) < 0.0)
            {
                return v += m;
            }
            else
            {
                return v;
            }
        }

        private static void matVecModM(double[,] A, double[] s, double[] v, double m)
        {
            double[] x = new double[3];
            for (int i = 0; i < 3; i++)
            {
                x[i] = multModM(A[i, 0], s[0], 0.0, m);
                x[i] = multModM(A[i, 1], s[1], x[i], m);
                x[i] = multModM(A[i, 2], s[2], x[i], m);
            }
            for (int i = 0; i < 3; i++)
            {
                v[i] = x[i];
            }
        }

        private static void matMatModM(double[,] A, double[,] B, double[,] C, double m)
        {
            double[] V = new double[3];
            double[,] W = new double[3, 3];

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    V[j] = B[j, i];
                }
                matVecModM(A, V, V, m);
                for (int j = 0; j < 3; j++)
                {
                    W[j, i] = V[j];
                }
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    C[i, j] = W[i, j];
                }
            }
        }

        private static void matTwoPowModM(double[,] A, double[,] B, double m, int e)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    B[i, j] = A[i, j];
                }
            }

            for (int i = 0; i < e; i++)
            {
                matMatModM(B, B, B, m);
            }
        }

        private static void matPowModM(double[,] A, double[,] B, double m, int c)
        {
            int n = c;
            double[,] W = new double[3, 3];

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    W[i, j] = A[i, j];
                    B[i, j] = 0.0;
                }
            }
            for (int j = 0; j < 3; j++)
            {
                B[j, j] = 1.0;
            }

            while (n > 0)
            {
                if (n % 2 == 1)
                {
                    matMatModM(W, B, B, m);
                }
                matMatModM(W, W, W, m);
                n /= 2;
            }
        }

        private double U01()
        {
            int k;
            double p1, p2, u;

            p1 = a12 * cg[1] - a13n * cg[0];
            k = (int)(p1 / m1);
            p1 -= k * m1;
            if (p1 < 0.0)
            {
                p1 += m1;
            }
            cg[0] = cg[1];
            cg[1] = cg[2];
            cg[2] = p1;

            p2 = a21 * cg[5] - a23n * cg[3];
            k = (int)(p2 / m2);
            p2 -= k * m2;
            if (p2 < 0.0)
            {
                p2 += m2;
            }
            cg[3] = cg[4];
            cg[4] = cg[5];
            cg[5] = p2;

            u = ((p1 > p2) ? (p1 - p2) * norm : (p1 - p2 + m1) * norm);
            return (anti) ? (1 - u) : u;
        }

        private double U01d()
        {
            double u = U01();
            if (anti)
            {
                u += (U01() - 1.0) * invtwo24;
                return (u < 0.0) ? u + 1.0 : u;
            }
            else
            {
                u += U01() * invtwo24;
                return (u < 1.0) ? u : (u - 1.0);
            }
        }

        public RngStream()
        {
            anti = false;
            prec53 = false;
            for (int i = 0; i < 6; i++)
            {
                bg[i] = cg[i] = ig[i] = nextSeed[i];
            }
            matVecModM(A1p127, nextSeed, nextSeed, m1);
            double[] temp = new double[3];
            for (int i = 0; i < 3; i++)
            {
                temp[i] = nextSeed[i + 3];
            }
            matVecModM(A2p127, temp, temp, m2);
            for (int i = 0; i < 3; i++)
            {
                nextSeed[i + 3] = temp[i];
            }
        }

        public static void SetPackageSeed(long[] seed)
        {
            for (int i = 0; i < 6; i++)
            {
                nextSeed[i] = seed[i];
            }
        }

        public static void SetPackageSeed(long seed)
        {
            for (int i = 0; i < 6; i++)
            {
                nextSeed[i] = seed;
            }
        }

        public void ResetStartStream()
        {
            for (int i = 0; i < 6; i++)
            {
                cg[i] = bg[i] = ig[i];
            }
        }

        public void ResetStartSubstream()
        {
            for (int i = 0; i < 6; i++)
            {
                cg[i] = bg[i];
            }
        }

        public void ResetNextSubstream()
        {
            matVecModM(A1p76, bg, bg, m1);
            double[] temp = new double[3];
            for (int i = 0; i < 3; i++)
            {
                temp[i] = bg[i + 3];
            }
            matVecModM(A2p76, temp, temp, m2);
            for (int i = 0; i < 3; i++)
            {
                bg[i + 3] = temp[i];
            }
            for (int i = 0; i < 6; i++)
            {
                cg[i] = bg[i];
            }
        }

        public void RetAntithetic(bool a)
        {
            anti = a;
        }

        public void IncreasedPrecis(bool incp)
        {
            prec53 = incp;
        }

        public void AdvanceState(int e, int c)
        {
            double[,] B1 = new double[3, 3];
            double[,] C1 = new double[3, 3];
            double[,] B2 = new double[3, 3];
            double[,] C2 = new double[3, 3];

            if (e > 0)
            {
                matTwoPowModM(A1p0, B1, m1, e);
                matTwoPowModM(A2p0, B2, m2, e);
            }
            else if (e < 0)
            {
                matTwoPowModM(InvA1, B1, m1, -e);
                matTwoPowModM(InvA2, B2, m2, -e);
            }

            if (c >= 0)
            {
                matPowModM(A1p0, C1, m1, c);
                matPowModM(A2p0, C2, m2, c);
            }
            else if (c < 0)
            {
                matPowModM(InvA1, C1, m1, -c);
                matPowModM(InvA2, C2, m2, -c);
            }

            if (e != 0)
            {
                matMatModM(B1, C1, C1, m1);
                matMatModM(B2, C2, C2, m2);
            }

            matVecModM(C1, cg, cg, m1);
            double[] cg3 = new double[3];
            for (int i = 0; i < 3; i++)
            {
                cg3[i] = cg[i + 3];
            }
            matVecModM(C2, cg3, cg3, m2);
            for (int i = 0; i < 3; i++)
            {
                cg[i + 3] = cg3[i];
            }
        }

        public void SetSeed(long[] seed)
        {
            for (int i = 0; i < 6; i++)
            {
                cg[i] = bg[i] = ig[i] = seed[i];
            }
        }

        public double RandU01()
        {
            if (prec53)
            {
                return U01d();
            }
            else
            {
                return U01();
            }
        }

        public int RandInt(int i, int j)
        {
            return (i + (int)(RandU01() * (j - i + 1.0)));
        }
    }

    [XmlType("RngStreamState")]
    public class RngStreamState
    {
        private double[] cg;

        [XmlArray("Cg")]
        [XmlArrayItem(typeof(double))]
        public double[] Cg
        {
            get { return this.cg; }
            set { this.cg = value; }
        }

        public RngStreamState()
        {
            this.cg = new double[6];
        }

        public void GetState(RngStream streamIn)
        {
            for (int i = 0; i < 6; i++)
            {
                this.cg[i] = streamIn.Cg[i];
            }
        }

        public void SetState(RngStream streamIn)
        {
            if (SimulationManagerState.FromExecutionToPlanning == false)
            {
                for (int i = 0; i < 6; i++)
                {
                    streamIn.Cg[i] = this.cg[i];
                }
            }
        }
    }
}