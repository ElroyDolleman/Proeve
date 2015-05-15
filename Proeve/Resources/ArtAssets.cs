﻿#define DEBUG

#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using E2DFramework.Graphics;
#endregion

namespace Proeve.Resources
{
    class ArtAssets
    {
        #region FILEPATHS
        private const string CHARACTERS_PATH = "Characters\\";
        private const string CHIPS_PATH = CHARACTERS_PATH + "Chips\\";

        private const string UI_PATH = "UI\\";
        private const string ARMY_EDITOR_UI_PATH = UI_PATH + "ArmyEditor\\";

        private const string BACKGROUND_PATH = "Backgrounds\\";

        private const string FONT_PATH = "Fonts\\";

        #endregion
        #region FILES

        /* CHARACTERS */
        // Chips
        private const string CHIPS_SHEET = "ChipsSheet";

        /* UI */
        // Buttons
        private const string TESTBUTTON = "startButtonPlaceHolder";

        // ArmyEditor
        private const string WEAPON_BUTTONS = "weaponButtons";

        /* BACKGROUND */
        private const string BACKGROUND_GRASS_LEVEL = "Background_grassland";
        private const string BACKGROUND_EDITOR = "editorBackground";

        /* FONTS */
        private const string FONT_NORMAL = "normalFont";

        #endregion
        #region TEXTURES

        /* CHARACTERS */
        // Chips Medieval
        private static E2DTexture chipsSheet;

        /* UI */
        // Buttons
        private static E2DTexture testButtonTexture;

        // Army Editor
        private static E2DTexture swordIconTexture;
        private static E2DTexture shieldIconTexture;
        private static E2DTexture axeIconTexture;

        /* BACKGROUNDS */
        public static E2DTexture editorBackground;
        public static E2DTexture backgroundGrassLevel;

        #endregion
        #region SPRITES

        /* CHARACTERS */
        // Chips
        private static Sprite medievalCaptainChipSprite;
        private static Sprite medievalMajoorChipSprite;
        private static Sprite medievalGeneralChipSprite;
        private static Sprite medievalMarshalChipSprite;

        private static Sprite medievalMinorChipSprite;
        private static Sprite medievalBombChipSprite;

        private static Sprite enemyChipSprite;

        /* UI */
        // Buttons
        private static Sprite testButtonSprite;

        // Army Editor
        private static Sprite axeIconSprite;
        private static Sprite swordIconSprite;
        private static Sprite shieldIconSprite;

        #endregion
        #region READONLY PROPERTIES

        /* CHARACTERS */
        // Chips
        public static Sprite MedievalCaptainChip { get { return (Sprite)medievalCaptainChipSprite.Clone(); } }
        public static Sprite MedievalMajoorChip { get { return (Sprite)medievalMajoorChipSprite.Clone(); } }
        public static Sprite MedievalGeneralChip { get { return (Sprite)medievalGeneralChipSprite.Clone(); } }
        public static Sprite MedievalMarshalChip { get { return (Sprite)medievalMarshalChipSprite.Clone(); } }

        public static Sprite MedievalMinorChip { get { return (Sprite)medievalMinorChipSprite.Clone(); } }
        public static Sprite MedievalBombChip { get { return (Sprite)medievalBombChipSprite.Clone(); } }

        public static Sprite EnemyChip { get { return (Sprite)enemyChipSprite.Clone(); } }

        /* UI */
        // Buttons
        public static Sprite TestButton { get { return (Sprite)testButtonSprite.Clone(); } }

        // ArmyEditor
        public static Sprite SwordIcon { get { return (Sprite)swordIconSprite; } }
        public static Sprite AxeIcon { get { return (Sprite)axeIconSprite; } }
        public static Sprite ShieldIcon { get { return (Sprite)shieldIconSprite; } }

        #endregion
        #region FONTS
        public static SpriteFont normalFont;
        #endregion

        public static void LoadTextures()
        {
            /* CHARACTERS */
            // Chips
            chipsSheet.Load(CHIPS_PATH, CHIPS_SHEET);

            /* UI */
            // Buttons
            testButtonTexture.Load(UI_PATH, TESTBUTTON);

            // Army Editor
            swordIconTexture.Load(ARMY_EDITOR_UI_PATH, WEAPON_BUTTONS);
            shieldIconTexture.Load(ARMY_EDITOR_UI_PATH, WEAPON_BUTTONS);
            axeIconTexture.Load(ARMY_EDITOR_UI_PATH, WEAPON_BUTTONS);

            /* BACKGROUNDS */
            editorBackground.Load(BACKGROUND_PATH, BACKGROUND_EDITOR);
            backgroundGrassLevel.Load(BACKGROUND_PATH, BACKGROUND_GRASS_LEVEL);

            InitializeSprites();
        }

        public static void LoadFont(ContentManager contentManager)
        {
            normalFont = contentManager.Load<SpriteFont>(FONT_PATH + FONT_NORMAL);

#if DEBUG
            Debug.spriteFont = Globals.contentManager.Load<SpriteFont>(FONT_PATH + FONT_NORMAL);
#endif
        }

        private static void InitializeSprites()
        {
            /* CHARACTERS */
            // Chips
            int W = 82;

            medievalCaptainChipSprite = new Sprite(chipsSheet, new Rectangle(W, 0, W, W));
            medievalMajoorChipSprite = new Sprite(chipsSheet, new Rectangle(0, 0, W, W));
            medievalGeneralChipSprite = new Sprite(chipsSheet, new Rectangle(W*2, W, W, W));
            medievalMarshalChipSprite = new Sprite(chipsSheet, new Rectangle(0, W, W, W));

            medievalMinorChipSprite = new Sprite(chipsSheet, new Rectangle(W*3, 0, W, W));
            medievalBombChipSprite = new Sprite(chipsSheet, new Rectangle(W*3, W, W, W));

            enemyChipSprite = new Sprite(chipsSheet, new Rectangle(W*8, 0, W, W));

            /* UI */
            // Buttons
            testButtonSprite = new Sprite(testButtonTexture, new Rectangle(0, 0, 64, 64), 2, 0f);

            // Army Editor
            axeIconSprite = new Sprite(swordIconTexture, new Rectangle(0, 82, 82, 82));
            swordIconSprite = new Sprite(swordIconTexture, new Rectangle(0, 0, 82, 82));
            shieldIconSprite = new Sprite(swordIconTexture, new Rectangle(0, 82*2, 82, 82));
        }
    }
}

