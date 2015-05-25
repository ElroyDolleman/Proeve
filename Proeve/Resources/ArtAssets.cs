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
        private const string FIGHTING_UI_PATH = UI_PATH + "Fighting\\";

        private const string BACKGROUND_PATH = "Backgrounds\\";

        private const string FONT_PATH = "Fonts\\";

        #endregion
        #region FILES

        /* CHARACTERS */
        // Chips
        private const string CHIPS_SHEET = "ChipsSheet";
        private const string ENEMY_CHIP_SHEET = "EnemyChips";

        /* UI */
        private const string UI_SHEET = "UI_sheet";

        // ArmyEditor

        // Fighting
        private const string DAMAGE_TEXT = "damage188x32";

        /* BACKGROUND */
        private const string BACKGROUND_GRASS_LEVEL = "backgroundGrassLand";
        private const string BACKGROUND_EDITOR = "editorBackground";

        /* FONTS */
        private const string FONT_NORMAL = "normalFont";

        #endregion
        #region TEXTURES

        /* CHARACTERS */
        // Chips Medieval
        private static E2DTexture chipsSheet;
        private static E2DTexture enemyChipSheet;

        /* UI */
        private static E2DTexture UISheet;

        // Army Editor

        // Figthing
        private static E2DTexture damageTextTexture;

        /* BACKGROUNDS */
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
        private static Sprite characterInformationUI;
        private static Sprite healthbar;
        private static Sprite stepCount;
        private static Sprite rankNamesNormal;
        private static Sprite diamondsUI;

        // Buttons
        private static Sprite startButtonSprite;
        private static Sprite endTurnButtonSprite;

        // Figthing
        private static Sprite damageTextSprite;
        private static Sprite fightingPopUp;

        // Army Editor
        private static Sprite armyShowCase;
        private static Sprite vsText;

        private static Sprite axeIconSprite;
        private static Sprite swordIconSprite;
        private static Sprite shieldIconSprite;

        #endregion
        #region READONLY PROPERTIES

        private static Point OnePixelInterspace { get { return new Point(1, 1); } }

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
        public static Sprite CharacterInformationUI { get { return (Sprite)characterInformationUI.Clone(); } }
        public static Sprite Healthbar { get { return (Sprite)healthbar.Clone(); } }
        public static Sprite StepCount { get { return (Sprite)stepCount.Clone(); } }
        public static Sprite RankNamesNormal { get { return (Sprite)rankNamesNormal.Clone(); } }

        // Buttons
        public static Sprite StartButton { get { return (Sprite)startButtonSprite.Clone(); } }
        public static Sprite EndTurnButton { get { return (Sprite)endTurnButtonSprite.Clone(); } }

        // ArmyEditor
        public static Sprite ArmyShowCase { get { return (Sprite)armyShowCase.Clone(); } }
        public static Sprite SwordIcon { get { return (Sprite)swordIconSprite.Clone(); } }
        public static Sprite AxeIcon { get { return (Sprite)axeIconSprite.Clone(); } }
        public static Sprite ShieldIcon { get { return (Sprite)shieldIconSprite.Clone(); } }

        // Figthing
        public static Sprite DamageTextSprite { get { return (Sprite)damageTextSprite.Clone(); } }
        public static Sprite FightPopUp { get { return (Sprite)fightingPopUp.Clone(); } }
        public static Sprite VSText { get { return (Sprite)vsText.Clone(); } }

        #endregion
        #region FONTS
#if DEBUG
        public static SpriteFont normalFont;
#endif
        #endregion

        public static void LoadTextures()
        {
            /* CHARACTERS */
            // Chips
            chipsSheet.Load(CHIPS_PATH, CHIPS_SHEET);
            enemyChipSheet.Load(CHIPS_PATH, ENEMY_CHIP_SHEET);

            /* UI */
            UISheet.Load(UI_PATH, UI_SHEET);

            // Fighting
            damageTextTexture.Load(FIGHTING_UI_PATH, DAMAGE_TEXT);

            // Army Editor
            

            /* BACKGROUNDS */
            backgroundGrassLevel.Load(BACKGROUND_PATH, BACKGROUND_GRASS_LEVEL);

            InitializeSprites();
        }

        public static void LoadFont(ContentManager contentManager)
        {
#if DEBUG
            normalFont = contentManager.Load<SpriteFont>(FONT_PATH + FONT_NORMAL);
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

            enemyChipSprite = new Sprite(enemyChipSheet, new Rectangle(0, 0, 85, 85), 25, 30f, 5);
            enemyChipSprite.origin = new Vector2(3, 3);

            /* UI */
            characterInformationUI = new Sprite(UISheet, new Rectangle(1602, 0, 261, 634));
            healthbar = new Sprite(UISheet, new Rectangle(1213, 681, 22, 37), 2); healthbar.Offset = OnePixelInterspace;
            stepCount = new Sprite(UISheet, new Rectangle(945, 756, 22, 28), 6); stepCount.Offset = OnePixelInterspace;
            rankNamesNormal = new Sprite(UISheet, new Rectangle(1327, 505, 141, 26), 8, 0f, 1, OnePixelInterspace);

            // Buttons
            startButtonSprite = new Sprite(UISheet, new Rectangle(0, 760, 324, 64));
            endTurnButtonSprite = new Sprite(UISheet, new Rectangle(325, 760, 198, 64));

            // Army Editor
            armyShowCase = new Sprite(UISheet, new Rectangle(945, 0, 656, 416));
            axeIconSprite = new Sprite(UISheet, new Rectangle(945, 417, 86, 87), 2); axeIconSprite.Offset = new Point(1, 0); axeIconSprite.origin = axeIconSprite.Center;
            swordIconSprite = new Sprite(UISheet, new Rectangle(1293, 417, 86, 87), 2); swordIconSprite.Offset = new Point(1, 0); swordIconSprite.origin = axeIconSprite.Center;
            shieldIconSprite = new Sprite(UISheet, new Rectangle(1119, 417, 86, 87), 2); shieldIconSprite.Offset = new Point(1, 0); shieldIconSprite.origin = axeIconSprite.Center;

            // Fighting
            damageTextSprite = new Sprite(damageTextTexture, new Rectangle(0, 0, 188, 32), 3, 0f, 1);
            damageTextSprite.origin = damageTextSprite.Center;

            vsText = new Sprite(UISheet, new Rectangle(524, 760, 86, 58));
            vsText.origin = vsText.Center;

            fightingPopUp = new Sprite(UISheet, new Rectangle(0, 0, 944, 759));
            fightingPopUp.origin = fightingPopUp.Center;
        }
    }
}

