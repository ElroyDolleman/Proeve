#define DEBUG

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
        private const string CAPTAIN_CHIP = "KapiteinRank4";
        private const string MAJOOR_CHIP = "MajoorRank5";
        private const string GENERAL_CHIP = "GeneraalRank6";
        private const string MARSHAL_CHIP = "MaarschalkRank7";

        private const string MINOR_CHIP = "MineurRank3";
        private const string BOMB_CHIP = "BombPlaceHolder";

        private const string ENEMY_CHIP = "EnemyChip";

        /* UI */
        // Buttons
        private const string TESTBUTTON = "startButtonPlaceHolder";

        // ArmyEditor
        private const string AXE_ICON = "AxeIcon";
        private const string SWORD_ICON = "swordIcon";
        private const string SHIELD_ICON = "shieldIcon";

        /* BACKGROUND */
        private const string BACKGROUND_GRASS_LEVEL = "environment_placeholder";
        private const string BACKGROUND_EDITOR = "editorBackground";

        /* FONTS */
        private const string FONT_NORMAL = "normalFont";

        #endregion
        #region TEXTURES

        /* CHARACTERS */
        // Chips
        private static E2DTexture captainChipTexture;
        private static E2DTexture majoorChipTexture;
        private static E2DTexture generalChipTexture;
        private static E2DTexture marshalChipTexture;

        private static E2DTexture minorChipTexture;
        private static E2DTexture bombChipTexture;

        private static E2DTexture enemyChipTexture;

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
        private static Sprite captainChipSprite;
        private static Sprite majoorChipSprite;
        private static Sprite generalChipSprite;
        private static Sprite marshalChipSprite;

        private static Sprite minorChipSprite;
        private static Sprite bombChipSprite;

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
        public static Sprite CaptainChip { get { return (Sprite)captainChipSprite.Clone(); } }
        public static Sprite MajoorChip { get { return (Sprite)majoorChipSprite.Clone(); } }
        public static Sprite GeneralChip { get { return (Sprite)generalChipSprite.Clone(); } }
        public static Sprite MarshalChip { get { return (Sprite)marshalChipSprite.Clone(); } }

        public static Sprite MinorChip { get { return (Sprite)minorChipSprite.Clone(); } }
        public static Sprite BombChip { get { return (Sprite)bombChipSprite.Clone(); } }

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
            captainChipTexture.Load(CHIPS_PATH, CAPTAIN_CHIP);
            majoorChipTexture.Load(CHIPS_PATH, MAJOOR_CHIP);
            generalChipTexture.Load(CHIPS_PATH, GENERAL_CHIP);
            marshalChipTexture.Load(CHIPS_PATH, MARSHAL_CHIP);

            minorChipTexture.Load(CHIPS_PATH, MINOR_CHIP);
            bombChipTexture.Load(CHIPS_PATH, BOMB_CHIP);

            enemyChipTexture.Load(CHIPS_PATH, ENEMY_CHIP);

            /* UI */
            // Buttons
            testButtonTexture.Load(UI_PATH, TESTBUTTON);

            // Army Editor
            swordIconTexture.Load(ARMY_EDITOR_UI_PATH, SWORD_ICON);
            shieldIconTexture.Load(ARMY_EDITOR_UI_PATH, SHIELD_ICON);
            axeIconTexture.Load(ARMY_EDITOR_UI_PATH, AXE_ICON);

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
            captainChipSprite = new Sprite(captainChipTexture);
            majoorChipSprite = new Sprite(majoorChipTexture);
            generalChipSprite = new Sprite(generalChipTexture);
            marshalChipSprite = new Sprite(marshalChipTexture);

            minorChipSprite = new Sprite(minorChipTexture);
            bombChipSprite = new Sprite(bombChipTexture);

            enemyChipSprite = new Sprite(enemyChipTexture);

            /* UI */
            // Buttons
            testButtonSprite = new Sprite(testButtonTexture, new Rectangle(0, 0, 64, 64), 2, 0f);

            // Army Editor
            axeIconSprite = new Sprite(swordIconTexture);
            swordIconSprite = new Sprite(swordIconTexture);
            shieldIconSprite = new Sprite(swordIconTexture);
        }
    }
}

