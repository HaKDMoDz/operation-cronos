using System;
using Microsoft.Xna.Framework;
using Operation_Cronos.Input;
using Operation_Cronos.IO;
using Microsoft.Xna.Framework.Input;
using Operation_Cronos.GameProcessor;

namespace Operation_Cronos.Display
{
    public partial class DisplayManager
    {

        #region IEventReceiver Members

        public void MousePress(object sender, MouseEventArgs args)
        {
            Point MouseWorldPosition = new Point();
            MouseWorldPosition.X = args.ScreenPosition.X + (int)camera.Position.X;
            MouseWorldPosition.Y = args.ScreenPosition.Y + (int)camera.Position.Y;

            if (root != null)
            {
                if (camera.ContainsPoint(MouseWorldPosition))
                    if (root.CollidesTile(MouseWorldPosition))
                    {
                        root.MousePress(args.Button, MouseWorldPosition);
                    }
            }
        }

        public void MouseRelease(object sender, MouseEventArgs args)
        {
            Point MouseWorldPosition = new Point();
            MouseWorldPosition.X = args.ScreenPosition.X + (int)camera.Position.X;
            MouseWorldPosition.Y = args.ScreenPosition.Y + (int)camera.Position.Y;

            if (root != null)
            {
                if (camera.ContainsPoint(MouseWorldPosition))
                    if (root.CollidesTile(MouseWorldPosition))
                    {
                        root.MouseRelease(args.Button, MouseWorldPosition);
                    }
            }
        }

        public void MouseWheel(object sender, MouseEventArgs args)
        {
            Point MouseWorldPosition = new Point();
            MouseWorldPosition.X = args.ScreenPosition.X + (int)camera.Position.X;
            MouseWorldPosition.Y = args.ScreenPosition.Y + (int)camera.Position.Y;
            if (root != null)
            {
                if (camera.ContainsPoint(MouseWorldPosition))
                    if (root.CollidesTile(MouseWorldPosition))
                    {
                        root.MouseWheel(args.Button, MouseWorldPosition);
                    }
            }
        }

        public void MouseMove(object sender, MouseEventArgs args)
        {
            Point MouseWorldPosition = new Point();
            MouseWorldPosition.X = args.ScreenPosition.X + (int)camera.Position.X;
            MouseWorldPosition.Y = args.ScreenPosition.Y + (int)camera.Position.Y;
            //
            if (pointer != null && camera!=null)
            {
                pointer.X = args.ScreenPosition.X - 13+(int)camera.Position.X;
                pointer.Y = args.ScreenPosition.Y - 3+(int)camera.Position.Y;
            }
            //
            if (root != null)
            {
                if (camera.ContainsPoint(MouseWorldPosition))
                {
                    root.MouseMove(args.Button, MouseWorldPosition);
                }
            }
        }

        public void GraphicsProgress(object sender, IOEventArgs args)
        {
            preloader.Percent = args.Percent;
        }

        public void GraphicsComplete(object sender, IOEventArgs args)
        {
            Debug.Clear();
            if (args.Operation == IOOperation.MainMenu)
            {
                IOManager.LoadFonts();
                CreateRoot();
            }
            if (args.Operation == IOOperation.CommandCenter)
            {
                CreateAndShowCommandCenter();
                preloader.Visible = false;
                preloader.Enabled = false;
                root.AddChild(preloader);
            }
            if (args.Operation == IOOperation.GameInterface_Graphics)
            {
                Debug.AddToLog("GameInterface Loaded");
                IOManager.LoadGameMapGraphics(); //once the game interface is loaded the GameMap Loading starts
            }
            if (args.Operation == IOOperation.GameMap_Graphics)
            {
                Debug.AddToLog("GameGraphics Loaded");
                InstanciateInterface(IOOperation.GameInterface_Instance);
            }
        }

        public void GraphicsNewPack(object sender, IOEventArgs args)
        {
        }

        public void GraphicsStart(object sender, IOEventArgs args)
        {
            preloader.Status = (IOOperation)Enum.Parse(typeof(IOOperation), args.Operation.ToString());
            CameraFreeze();
            preloader.Visible = true;
            preloader.Enabled = true;
        }

        public void KeyPress(object sender, KeyboardEventArgs args)
        {
            StartMovingCamera(args.Key);

            //---MainMenu------------------------------------------
            if (mainMenu != null)
            if (mainMenu.inputNewGameUserName != null)
                mainMenu.inputNewGameUserName.InputKeyCode(args.Key);
            //------------------------------------------------------
            if (args.Key == Keys.Escape)
            {
                Debug.Clear();
            }
            if (args.Key == Keys.C)
            {
                GameManager.SetMoney(GameManager.CurrentYear, 0);
                GUI.SetMoney(0);
            }
            if (args.Key == Keys.X)
            {
                GameManager.AddMoney(GameManager.CurrentYear, 1000);
                GUI.SetMoney(GameManager.GetMoney(GameManager.CurrentYear));
            }
        }

        public void KeyRelease(object sender, KeyboardEventArgs args)
        {
            StopMovingCamera();            
        }

        public void FontsStart(object sender, IOEventArgs args) { }

        public void FontsProgress(object sender, IOEventArgs args)
        {

            preloader.Percent = args.Percent;
        }

        public void FontsComplete(object sender, IOEventArgs args)
        {
            preloader.Visible = false;
            preloader.Enabled = false;
            CreateAndShowMainMenu();
        }
        #endregion
    }
}