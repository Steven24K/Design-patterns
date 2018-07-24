using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AssignmentComplete
{
  class Mine : IFactory
  {

    class AddOreBoxToMine : IAction
    {
      Mine mine;
      public AddOreBoxToMine(Mine mine)
      {
        this.mine = mine;
      }
      public void Run()
      {
        if (mine.ProductsToShip.Count < 3)
           mine.ProductsToShip.Add(CreateOreBox(mine.Position + new Vector2(-80, 40 + -30 * mine.ProductsToShip.Count)));
                else
                {
                    mine.ProductsToShip.Clear();
                }
      }
      Ore CreateOreBox(Vector2 position)
      {
        var box = new Ore(100, mine.oreBox);
        box.Position = position;
        return box;
      }
    }

    Texture2D mine, oreContainer, oreBox, truckTexture;
    List<IStateMachine> processes;
    ITruck waitingTruck;
    bool isTruckReady = false;
    Vector2 position;
    List<IContainer> productsToShip;
        

    public Mine(Vector2 position, Texture2D truck_texture, Texture2D mine, Texture2D ore_box, Texture2D ore_container)
    {
      processes = new List<IStateMachine>();
      ProductsToShip = new List<IContainer>();
      this.mine = mine;
      this.truckTexture = truck_texture;
      this.oreContainer = ore_container;
      this.oreBox = ore_box;
      this.position = position;
      this.waitingTruck = null;

            processes.Add(
              new Repeat(new Seq(new Timer(1.0f),
                                 new Call(new AddOreBoxToMine(this)))));



            processes.Add(new Repeat(new Seq(new Wait(() => ProductsToShip.Count == 0),
                new Seq(new Timer(1.0f), new CallAction(() => waitingTruck = new Truck(null, Position + new Vector2(100, 30), new Vector2(0, 0), truckTexture))))));

            processes.Add(new Repeat(new Seq(new Wait(() => ProductsToShip.Count == 3),
                new Seq(new Timer(1.0f), new CallAction(() => waitingTruck.AddContainer(new Ore(0, oreContainer)))))));


        }


        public ITruck GetReadyTruck()
    {
            return waitingTruck;
    }

    public Vector2 Position
    {
      get
      {
        return position;
      }
    }
    public List<IContainer> ProductsToShip
    {
      get
      {
        return productsToShip;
      }
      set
      {
        productsToShip = value;
      }
    }
    public void Draw(SpriteBatch spriteBatch)
    {
      foreach (var cart in ProductsToShip)
      {
        cart.Draw(spriteBatch);
      }
      spriteBatch.Draw(mine, Position, Color.White);
    }
    public void Update(float dt)
    {
      foreach (var process in processes)
      {
        process.Update(dt);
      }
    }
    
  }


    class Ikea : IFactory
    {

        class AddProductBoxtoIkea : IAction
        {
            Ikea ikea;
            public AddProductBoxtoIkea(Ikea ikea)
            {
                this.ikea = ikea;
            }
            public void Run()
            {
                if (ikea.ProductsToShip.Count < 3)
                    ikea.ProductsToShip.Add(CreateProductBox(ikea.Position + new Vector2(-80, 40 + -30 * ikea.ProductsToShip.Count)));
                else
                {
                    ikea.ProductsToShip.Clear();
                }
            }
            Ore CreateProductBox(Vector2 position)
            {
                var box = new Ore(100, ikea.productBox);
                box.Position = position;
                return box;
            }
        }

        Texture2D ikea, ProductContainer, productBox, truckTexture;
        List<IStateMachine> processes;
        ITruck waitingTruck;
        bool isTruckReady = false;
        Vector2 position;
        List<IContainer> productsToShip;


        public Ikea(Vector2 position, Texture2D truck_texture, Texture2D ikea, Texture2D product_box, Texture2D product_container)
        {
            processes = new List<IStateMachine>();
            ProductsToShip = new List<IContainer>();
            this.ikea = ikea;
            this.truckTexture = truck_texture;
            this.ProductContainer = product_container;
            this.productBox = product_box;
            this.position = position;
            this.waitingTruck = null;

            processes.Add(
              new Repeat(new Seq(new Timer(1.0f),
                                 new Call(new AddProductBoxtoIkea(this)))));



            processes.Add(new Repeat(new Seq(new Wait(() => ProductsToShip.Count == 0),
                new Seq(new Timer(1.0f), new CallAction(() => waitingTruck = new Truck(null, Position + new Vector2(100, 30), new Vector2(0, 0), truckTexture))))));

            processes.Add(new Repeat(new Seq(new Wait(() => ProductsToShip.Count == 3),
                new Seq(new Timer(1.0f), new CallAction(() => waitingTruck.AddContainer(new Ore(0, ProductContainer)))))));


        }


        public ITruck GetReadyTruck()
        {
            return waitingTruck;
        }

        public Vector2 Position
        {
            get
            {
                return position;
            }
        }
        public List<IContainer> ProductsToShip
        {
            get
            {
                return productsToShip;
            }
            set
            {
                productsToShip = value;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var cart in ProductsToShip)
            {
                cart.Draw(spriteBatch);
            }
            spriteBatch.Draw(ikea, Position, Color.White);
        }
        public void Update(float dt)
        {
            foreach (var process in processes)
            {
                process.Update(dt);
            }
        }

    }
}
