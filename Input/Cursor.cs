﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Rampastring.XNAUI.Input
{
    public class Cursor : DrawableGameComponent
    {
        public Cursor(WindowManager windowManager)
            : base(windowManager.Game)
        {
            previousMouseState = Mouse.GetState();
            RemapColor = Color.White;
            this.windowManager = windowManager;
        }

        public event EventHandler LeftClickEvent;

        public Point Location { get; set; }
        Point DrawnLocation { get; set; }

        public bool HasMoved { get; private set; }
        public bool IsOnScreen { get; private set; }

        public Texture2D[] Textures;

        public int TextureIndex { get; set; }

        public bool LeftClicked { get; private set; }

        public bool RightClicked { get; private set; }

        /// <summary>
        /// Gets a value that indicates whether the left mouse button is held down
        /// on the current frame.
        /// </summary>
        public bool LeftDown { get; private set; }

        /// <summary>
        /// Gets a value that indicates whether the left mouse button was pressed
        /// down on this frame (meaning it's down on the current frame, but wasn't down
        /// on the previous frame).
        /// </summary>
        public bool LeftPressedDown { get; private set; }

        /// <summary>
        /// Gets a value that indicates whether the right mouse button was pressed
        /// down on this frame (meaning it's down on the current frame, but wasn't down
        /// on the previous frame).
        /// </summary>
        public bool RightPressedDown { get; private set; }

        public bool HasFocus { get; set; }

        public bool Disabled { get; set; }

        public int ScrollWheelValue { get; set; }

        public Color RemapColor { get; set; }

        WindowManager windowManager;
        MouseState previousMouseState;

        public override void Initialize()
        {
        }

        public override void Update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();

#if WINDOWSGL
            DrawnLocation = new Point(ms.X, ms.Y) - Game.Window.ClientBounds.Location;
#else
            DrawnLocation = new Point(ms.X, ms.Y);
#endif

            if (!HasFocus || Disabled)
            {
                LeftClicked = false;
                RightClicked = false;
                LeftDown = false;
                return;
            }

            Point location = DrawnLocation;

            IsOnScreen = !(location.X < 0 || location.Y < 0 ||
                location.X > windowManager.ResolutionWidth ||
                location.Y > windowManager.ResolutionHeight);

            location = new Point(location.X - windowManager.SceneXPosition, location.Y - windowManager.SceneYPosition);
            location = new Point((int)(location.X / windowManager.ScaleRatio), (int)(location.Y / windowManager.ScaleRatio));

            HasMoved = (location != Location);

            Location = location;

            ScrollWheelValue = (ms.ScrollWheelValue - previousMouseState.ScrollWheelValue) / 40;

            LeftDown = ms.LeftButton == ButtonState.Pressed;
            LeftPressedDown = LeftDown && previousMouseState.LeftButton != ButtonState.Pressed;

            LeftClicked = !LeftDown && previousMouseState.LeftButton == ButtonState.Pressed;

            if (LeftClicked)
                LeftClickEvent?.Invoke(this, EventArgs.Empty);

            RightPressedDown = ms.RightButton == ButtonState.Pressed && previousMouseState.RightButton != ButtonState.Pressed;
            RightClicked = ms.RightButton == ButtonState.Released && previousMouseState.RightButton == ButtonState.Pressed;

            previousMouseState = ms;
        }

        public override void Draw(GameTime gameTime)
        {
            Texture2D texture = Textures[TextureIndex];

            Renderer.DrawTexture(texture,
                new Rectangle(DrawnLocation.X, DrawnLocation.Y, texture.Width, texture.Height), RemapColor);
        }
    }
}
