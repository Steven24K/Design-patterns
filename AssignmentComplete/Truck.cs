using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AssignmentComplete
{
  class Truck : ITruck
    {
        IContainer container;
        Vector2 position, velocity;
        Texture2D TruckTexture;

        public Truck(IContainer container, Vector2 pos, Vector2 speed, Texture2D texture)
        {
            this.container = container;
            this.position = pos;
            this.velocity = speed;
            this.TruckTexture = texture;
        }

        public IContainer Container { get { return this.container; } set { this.container = value; } }

        public Vector2 Position { get { return this.position; }set { this.position = value; } }

        public Vector2 Velocity { get { return this.velocity; } set { this.velocity = value; } }

        public void StartEngine()
        {
            this.velocity = new Vector2(1,0);
        }

        public void AddContainer(IContainer container)
        {
            this.Container = container;
            this.Container.Position = this.Position;
        }

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(TruckTexture, Position, Color.White);
            if (Container != null)
                Container.Draw(spritebatch);
        }

        public void Update(float dt)
        {
            this.Position = this.Position + this.Velocity;
            if (Container != null)
                this.Container.Position = Position + Velocity + new Vector2(-12, -12);
        }
    }
}
