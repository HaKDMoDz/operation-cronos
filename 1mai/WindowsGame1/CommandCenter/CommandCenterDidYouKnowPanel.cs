using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Operation_Cronos.Display;
using Operation_Cronos.Input;
using Operation_Cronos.IO;

namespace Operation_Cronos
{
    class CommandCenterDidYouKnowPanel : InputVisualComponent
    {
        #region Fields
        GraphicsCollection graphicsCollection;

        CommandCenterGeneralButton btnTab1;
        CommandCenterGeneralButton btnTab2;
        CommandCenterGeneralButton btnTab3;
        CommandCenterGeneralButton btnTab4;
        CommandCenterGeneralButton btnTab5;
        CommandCenterGeneralButton btnTab6;

        List<CommandCenterGeneralButton> btnTabs;

        Sprite spritePage1;
        Sprite spritePage2;
        Sprite spritePage3;
        Sprite spritePage4;
        Sprite spritePage5;
        Sprite spritePage6;

        int spritePagesXPoz = 52;
        int spritePagesYPoz = 125;

        List<Sprite> spritePages;

        Scrollbar tab4Scrollbar;
        #endregion

        #region Constructors
        public CommandCenterDidYouKnowPanel(Game game, int DrawOrder)
            : base(game)
        {
            graphicsCollection = (GraphicsCollection)Game.Services.GetService(typeof(GraphicsCollection));
            btnTabs = new List<CommandCenterGeneralButton>();
            spritePages = new List<Sprite>();

            btnTab1 = new CommandCenterGeneralButton(game, new Sprite(game, graphicsCollection.GetPack("DidYouKnowPanel_Tab1")), 1);
            btnTab1.Position = new Point(spritePagesXPoz + 14, spritePagesYPoz - 32);
            btnTab1.OnPress += new EventHandler<ButtonEventArgs>(btnTab_OnPress);
            btnTabs.Add(btnTab1);
            AddChild(btnTab1);

            btnTab2 = new CommandCenterGeneralButton(game, new Sprite(game, graphicsCollection.GetPack("DidYouKnowPanel_Tab2")), 1);
            btnTab2.Position = new Point(spritePagesXPoz + 14 + 125, spritePagesYPoz - 32);
            btnTab2.OnPress += new EventHandler<ButtonEventArgs>(btnTab_OnPress);
            btnTabs.Add(btnTab2);
            AddChild(btnTab2);

            btnTab3 = new CommandCenterGeneralButton(game, new Sprite(game, graphicsCollection.GetPack("DidYouKnowPanel_Tab3")), 1);
            btnTab3.Position = new Point(spritePagesXPoz + 14 + (125 * 2), spritePagesYPoz - 32);
            btnTab3.OnPress += new EventHandler<ButtonEventArgs>(btnTab_OnPress);
            btnTabs.Add(btnTab3);
            AddChild(btnTab3);

            btnTab4 = new CommandCenterGeneralButton(game, new Sprite(game, graphicsCollection.GetPack("DidYouKnowPanel_Tab4")), 1);
            btnTab4.Position = new Point(spritePagesXPoz + 14 + (125 * 3), spritePagesYPoz - 32);
            btnTab4.OnPress += new EventHandler<ButtonEventArgs>(btnTab_OnPress);
            btnTabs.Add(btnTab4);
            AddChild(btnTab4);

            btnTab5 = new CommandCenterGeneralButton(game, new Sprite(game, graphicsCollection.GetPack("DidYouKnowPanel_Tab5")), 1);
            btnTab5.Position = new Point(spritePagesXPoz + 14 + (125 * 4), spritePagesYPoz - 32);
            btnTab5.OnPress += new EventHandler<ButtonEventArgs>(btnTab_OnPress);
            btnTabs.Add(btnTab5);
            AddChild(btnTab5);

            btnTab6 = new CommandCenterGeneralButton(game, new Sprite(game, graphicsCollection.GetPack("DidYouKnowPanel_Tab6")), 1);
            btnTab6.Position = new Point(spritePagesXPoz + 14 + (125 * 5), spritePagesYPoz - 32);
            btnTab6.OnPress += new EventHandler<ButtonEventArgs>(btnTab_OnPress);
            btnTabs.Add(btnTab6);
            AddChild(btnTab6);

            spritePage1 = new Sprite(game, graphicsCollection.GetPack("DidYouKnowPanel_Page1"));
            spritePage1.StackOrder = 0;
            spritePage1.XRelative = spritePagesXPoz;
            spritePage1.YRelative = spritePagesYPoz;
            spritePages.Add(spritePage1);
            AddChild(spritePage1);

            spritePage2 = new Sprite(game, graphicsCollection.GetPack("DidYouKnowPanel_Page2"));
            spritePage2.StackOrder = 0;
            spritePage2.XRelative = spritePagesXPoz;
            spritePage2.YRelative = spritePagesYPoz;
            spritePages.Add(spritePage2);
            AddChild(spritePage2);

            spritePage3 = new Sprite(game, graphicsCollection.GetPack("DidYouKnowPanel_Page3"));
            spritePage3.StackOrder = 0;
            spritePage3.XRelative = spritePagesXPoz;
            spritePage3.YRelative = spritePagesYPoz;
            spritePages.Add(spritePage3);
            AddChild(spritePage3);

            spritePage4 = new Sprite(game, graphicsCollection.GetPack("DidYouKnowPanel_Page4"));
            spritePage4.StackOrder = 0;
            spritePage4.XRelative = spritePagesXPoz;
            spritePage4.YRelative = spritePagesYPoz;
            spritePages.Add(spritePage4);
            AddChild(spritePage4);

            spritePage5 = new Sprite(game, graphicsCollection.GetPack("DidYouKnowPanel_Page5"));
            spritePage5.StackOrder = 0;
            spritePage5.XRelative = spritePagesXPoz;
            spritePage5.YRelative = spritePagesYPoz;
            spritePages.Add(spritePage5);
            AddChild(spritePage5);

            spritePage6 = new Sprite(game, graphicsCollection.GetPack("DidYouKnowPanel_Page6"));
            spritePage6.StackOrder = 0;
            spritePage6.XRelative = spritePagesXPoz;
            spritePage6.YRelative = spritePagesYPoz;
            spritePages.Add(spritePage6);
            AddChild(spritePage6);

            this.StackOrder = DrawOrder;

            this.Hide();
        }
        #endregion

        #region Event Handlers

        void btnTab_OnPress(object sender, ButtonEventArgs e)
        {
            for (int i = 0; i < btnTabs.Count; i++)
            {
                if (btnTabs[i] != ((CommandCenterGeneralButton)sender)) //not the button that launched the event
                {
                    if (btnTabs[i].IsPressed)//and currently pressed (previosly selected)
                    {
                        btnTabs[i].ReleaseButton();//it is released
                        //hides the previously selected button's page
                        spritePages[i].FrameNumber = 0;
                        spritePages[i].Visible = false;
                    }
                }
                else // the button that Launched the Event
                {
                    spritePages[i].Visible = true;
                }
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Hides the entire Help Panel
        /// </summary>
        public void Hide()
        {
            for (int i = 0; i < btnTabs.Count; i++)
            {
                btnTabs[i].Hide();
                btnTabs[i].ReleaseButton();
                spritePages[i].Visible = false;
                spritePages[i].FrameNumber = 0;
            }

            this.Enabled = false;
        }

        /// <summary>
        /// Shows the Help Panel
        /// </summary>
        public void Show()
        {
            for (int i = 0; i < btnTabs.Count; i++)
            {
                btnTabs[i].Show();
                btnTabs[i].ReleaseButton();
                spritePages[i].Visible = false;
            }
            btnTabs[0].Press(MouseButton.LeftButton);

            this.Enabled = true;
        }
        #endregion

        #region Overrides
        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        #endregion
    }
}

