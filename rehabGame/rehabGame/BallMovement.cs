using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
namespace rehabGame
{
    class BallMovement : BasicModel
    {
        Matrix rotation = Matrix.Identity;
        Vector3 position = new Vector3(0, -4, 0);
        
        public BallMovement(Model m)
            : base(m)
        {
            
        }

        public override void Update()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                position += Vector3.Left * 0.2F;
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                position += Vector3.Right * 0.2F;
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                position += Vector3.Forward * 0.2F;
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                position += Vector3.Backward * 0.2F;

            //Move model
            world = Matrix.CreateTranslation(position);   
        }

        public override Matrix GetWorld()
        {
            return world * rotation;
        }
    }
}