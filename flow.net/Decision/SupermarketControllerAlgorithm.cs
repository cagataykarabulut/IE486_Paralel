using System;
using System.Collections.Generic;
using System.Text;
using FLOW.NET.Layout;
using FLOW.NET.Operational;

//IE486 Fall19
namespace FLOW.NET.Decision
{
    public abstract class SupermarketControllerAlgorithm : DecisionAlgorithm
    {
        public void Execute()
        {
            this.PreOperation();
            SupermarketList supermarketList = new SupermarketList();

            if (this.Manager.LayoutManager.SupermarketsToDecide != null)
            {
                supermarketList = this.Manager.LayoutManager.SupermarketsToDecide;
            }

            foreach (Supermarket supermarket in supermarketList)
            {
                this.ControlSupermarket(supermarket);
            }

            this.PostOperation();
        }

        protected virtual void PostOperation() { }

        protected virtual void PreOperation() { }

        //key function to define controlling strategies for different implementations
        protected abstract void ControlSupermarket(Supermarket supermarketIn); // will be written differently for each type of control strategy

        public static SupermarketControllerAlgorithm GetAlgorithmByName(string nameIn)
        {
            //add your algorithm names here for initialization
            switch (nameIn)
            {
                case "MixedSupermarketConfiguration":
                    return new SupermarketController.MixedSupermarketConfiguration();
                default:
                    throw new Exception("Invalid supermarket controller algorithm name.");
            }
        }

        public static StringList GetAlgorithmNames()
        {

            StringList names = new StringList();
            names.Add("MixedSupermarketConfiguration");
            return names;

        }

    }
}
