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

        /* UI */
        private const string UI_SHEET = "UI_sheet";

        // ArmyEditor
        private const string ARMY_SHEET = "Army_editor_sheet_115x115";

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

        /* UI */
        private static E2DTexture UISheet;

        // Army Editor
        private static E2DTexture armySheetTexture;

        // Figthing
        private static E2DTexture damageTextTexture;

        /* BACKGROUNDS */
        public static E2DTexture backgroundGrassLevel;

        #endregion
        #region SPRITES

        /* CHARACTERS */
        // Chips
        private static Sprite medievalSoldierChipSprite;
        private static Sprite medievalCaptainChipSprite;
        private static Sprite medievalGeneralChipSprite;
        private static Sprite medievalLeaderChipSprite;

        private static Sprite medievalSpyChipSprite;
        private static Sprite medievalMinerChipSprite;
        private static Sprite medievalHealerChipSprite;
        private static Sprite medievalBombChipSprite;

        private static Sprite tikiSoldierChipSprite;
        private static Sprite tikiCaptainChipSprite;
        private static Sprite tikiGeneralChipSprite;
        private static Sprite tikiLeaderChipSprite;

        private static Sprite tikiSpyChipSprite;
        private static Sprite tikiMinerChipSprite;
        private static Sprite tikiHealerChipSprite;
        private static Sprite tikiBombChipSprite;

        private static Sprite enemyChipSprite;

        /* UI */
        private static Sprite characterInformationUI;
        private static Sprite healthbar;
        private static Sprite stepCount;
        private static Sprite diamondsUI;
        private static Sprite numbers;
        private static Sprite bigNumbers;

        private static Sprite rankNamesNormal;
        private static Sprite rankNamesBold;

        // Buttons
        private static Sprite startButtonSprite;
        private static Sprite endTurnButtonSprite;
        private static Sprite homeButton;

        // Figthing
        private static Sprite damageTextSprite;
        private static Sprite fightingPopUp;

        // Army Editor
        private static Sprite armyShowCase;
        private static Sprite armyNames;
        private static Sprite vsText;

        private static Sprite axeIconSprite;
        private static Sprite swordIconSprite;
        private static Sprite shieldIconSprite;

        private static Sprite medievalArmySheet;
        private static Sprite tikiArmySheet;

        // Win Lose
        private static Sprite winPopUp, losePopUp;
        private static Sprite rewardText;
        private static Sprite returnButton;

        #endregion
        #region READONLY PROPERTIES

        private static Point OnePixelInterspace { get { return new Point(1, 1); } }

        /* CHARACTERS */
        // Chips
        public static Sprite MedievalSoldierChip { get { return (Sprite)medievalSoldierChipSprite.Clone(); } }
        public static Sprite MedievalCaptainChip { get { return (Sprite)medievalCaptainChipSprite.Clone(); } }
        public static Sprite MedievalGeneralChip { get { return (Sprite)medievalGeneralChipSprite.Clone(); } }
        public static Sprite MedievalLeaderChip { get { return (Sprite)medievalLeaderChipSprite.Clone(); } }

        public static Sprite MedievalSpyChip { get { return (Sprite)medievalSpyChipSprite.Clone(); } }
        public static Sprite MedievalMinerChip { get { return (Sprite)medievalMinerChipSprite.Clone(); } }
        public static Sprite MedievalHealerChip { get { return (Sprite)medievalHealerChipSprite.Clone(); } }
        public static Sprite MedievalBombChip { get { return (Sprite)medievalBombChipSprite.Clone(); } }

        public static Sprite TikiSoldierChip { get { return (Sprite)tikiSoldierChipSprite.Clone(); } }
        public static Sprite TikiCaptainChip { get { return (Sprite)tikiCaptainChipSprite.Clone(); } }
        public static Sprite TikiGeneralChip { get { return (Sprite)tikiGeneralChipSprite.Clone(); } }
        public static Sprite TikiLeaderChip { get { return (Sprite)tikiLeaderChipSprite.Clone(); } }

        public static Sprite TikiSpyChip { get { return (Sprite)tikiSpyChipSprite.Clone(); } }
        public static Sprite TikiMinerChip { get { return (Sprite)tikiMinerChipSprite.Clone(); } }
        public static Sprite TikiHealerChip { get { return (Sprite)tikiHealerChipSprite.Clone(); } }
        public static Sprite TikiBombChip { get { return (Sprite)tikiBombChipSprite.Clone(); } }

        public static Sprite EnemyChip { get { return (Sprite)enemyChipSprite.Clone(); } }

        /* UI */
        public static Sprite CharacterInformationUI { get { return (Sprite)characterInformationUI.Clone(); } }
        public static Sprite Healthbar { get { return (Sprite)healthbar.Clone(); } }
        public static Sprite StepCount { get { return (Sprite)stepCount.Clone(); } }

        public static Sprite RankNamesNormal { get { return (Sprite)rankNamesNormal.Clone(); } }
        public static Sprite RankNamesBold { get { return (Sprite)rankNamesBold.Clone(); } }

        public static Sprite DiamondsUI { get { return (Sprite)diamondsUI.Clone(); } }
        public static Sprite Numbers { get { return (Sprite)numbers.Clone(); } }

        // Buttons
        public static Sprite StartButton { get { return (Sprite)startButtonSprite.Clone(); } }
        public static Sprite EndTurnButton { get { return (Sprite)endTurnButtonSprite.Clone(); } }
        public static Sprite HomeButton { get { return (Sprite)homeButton.Clone(); } }

        // ArmyEditor
        public static Sprite ArmyShowCase { get { return (Sprite)armyShowCase.Clone(); } }
        public static Sprite ArmyNames { get { return (Sprite)armyNames.Clone(); } }
        public static Sprite SwordIcon { get { return (Sprite)swordIconSprite.Clone(); } }
        public static Sprite AxeIcon { get { return (Sprite)axeIconSprite.Clone(); } }
        public static Sprite ShieldIcon { get { return (Sprite)shieldIconSprite.Clone(); } }

        public static Sprite MedievalArmySheet { get { return (Sprite)medievalArmySheet.Clone(); } }
        public static Sprite TikiArmySheet { get { return (Sprite)tikiArmySheet.Clone(); } }

        // Figthing
        public static Sprite DamageTextSprite { get { return (Sprite)damageTextSprite.Clone(); } }
        public static Sprite FightPopUp { get { return (Sprite)fightingPopUp.Clone(); } }
        public static Sprite VSText { get { return (Sprite)vsText.Clone(); } }

        // Win/Lose
        public static Sprite WinPopUp { get { return (Sprite)winPopUp.Clone(); } }
        public static Sprite LosePopUp { get { return (Sprite)losePopUp.Clone(); } }
        public static Sprite ReturnButton { get { return (Sprite)returnButton.Clone(); } }

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

            /* UI */
            UISheet.Load(UI_PATH, UI_SHEET);

            // Fighting
            damageTextTexture.Load(FIGHTING_UI_PATH, DAMAGE_TEXT);

            // Army Editor
            armySheetTexture.Load(ARMY_EDITOR_UI_PATH, ARMY_SHEET);

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

            medievalSoldierChipSprite = new Sprite(chipsSheet, new Rectangle(W, 0, W, W));
            medievalCaptainChipSprite = new Sprite(chipsSheet, new Rectangle(0, 0, W, W));
            medievalGeneralChipSprite = new Sprite(chipsSheet, new Rectangle(W*2, W, W, W));
            medievalLeaderChipSprite = new Sprite(chipsSheet, new Rectangle(0, W, W, W));

            medievalSpyChipSprite = new Sprite(chipsSheet, new Rectangle(W * 2, 0, W, W));
            medievalMinerChipSprite = new Sprite(chipsSheet, new Rectangle(W * 3, 0, W, W));
            medievalHealerChipSprite = new Sprite(chipsSheet, new Rectangle(W, W, W, W));
            medievalBombChipSprite = new Sprite(chipsSheet, new Rectangle(W*3, W, W, W));

            tikiSoldierChipSprite = new Sprite(chipsSheet, new Rectangle(W*5, 0, W, W));
            tikiCaptainChipSprite = new Sprite(chipsSheet, new Rectangle(W*4, 0, W, W));
            tikiGeneralChipSprite = new Sprite(chipsSheet, new Rectangle(W*6, W, W, W));
            tikiLeaderChipSprite = new Sprite(chipsSheet, new Rectangle(W*4, W, W, W));

            tikiSpyChipSprite = new Sprite(chipsSheet, new Rectangle(W*6, 0, W, W));
            tikiMinerChipSprite = new Sprite(chipsSheet, new Rectangle(W*7, 0, W, W));
            tikiHealerChipSprite = new Sprite(chipsSheet, new Rectangle(W*5, W, W, W));
            tikiBombChipSprite = new Sprite(chipsSheet, new Rectangle(W*7, W, W, W));

            enemyChipSprite = new Sprite(chipsSheet, new Rectangle(W*8, 0, 82, 82));

            /* UI */
            characterInformationUI = new Sprite(UISheet, new Rectangle(1602, 0, 261, 634));
            healthbar = new Sprite(UISheet, new Rectangle(1213, 681, 22, 37), 2); healthbar.Offset = OnePixelInterspace;
            stepCount = new Sprite(UISheet, new Rectangle(945, 756, 22, 28), 6); stepCount.Offset = OnePixelInterspace;

            rankNamesNormal = new Sprite(UISheet, new Rectangle(1327, 505, 141, 26), 8, 0f, 1, OnePixelInterspace);
            rankNamesBold = new Sprite(UISheet, new Rectangle(1213, 505, 113, 21), 8, 0f, 1, OnePixelInterspace);

            numbers = new Sprite(UISheet, new Rectangle(945, 785, 15, 17), 10, 0f, 0, OnePixelInterspace);

            // Buttons
            startButtonSprite = new Sprite(UISheet, new Rectangle(0, 760, 324, 64));
            endTurnButtonSprite = new Sprite(UISheet, new Rectangle(325, 760, 198, 64));
            homeButton = new Sprite(UISheet, new Rectangle(778, 760, 86, 86));

            // Army Editor
            armyShowCase = new Sprite(UISheet, new Rectangle(945, 0, 656, 416));
            axeIconSprite = new Sprite(UISheet, new Rectangle(945, 417, 86, 87), 2); axeIconSprite.Offset = new Point(1, 0); axeIconSprite.origin = axeIconSprite.Center;
            swordIconSprite = new Sprite(UISheet, new Rectangle(1293, 417, 86, 87), 2); swordIconSprite.Offset = new Point(1, 0); swordIconSprite.origin = axeIconSprite.Center;
            shieldIconSprite = new Sprite(UISheet, new Rectangle(1119, 417, 86, 87), 2); shieldIconSprite.Offset = new Point(1, 0); shieldIconSprite.origin = axeIconSprite.Center;

            medievalArmySheet = new Sprite(armySheetTexture, new Rectangle(0, 0, 115, 115), 8);
            tikiArmySheet = new Sprite(armySheetTexture, new Rectangle(0, 0, 115, 115), 8); tikiArmySheet.SheetPosition = new Point(0, 115);

            armyNames = new Sprite(UISheet, new Rectangle(945, 505, 267, 41), 3, 0f, 1);

            // Fighting
            damageTextSprite = new Sprite(damageTextTexture, new Rectangle(0, 0, 188, 32), 5, 0f, 1);
            damageTextSprite.origin = damageTextSprite.Center;

            vsText = new Sprite(UISheet, new Rectangle(524, 760, 86, 58));
            vsText.origin = vsText.Center;

            fightingPopUp = new Sprite(UISheet, new Rectangle(0, 0, 944, 759));
            fightingPopUp.origin = fightingPopUp.Center;

            diamondsUI = new Sprite(UISheet, new Rectangle(611, 760, 166, 75));

            // Win / Lose
            winPopUp = new Sprite(UISheet, new Rectangle(0, 845, 906, 687)); //winPopUp.origin = winPopUp.Center;
            losePopUp = new Sprite(UISheet, new Rectangle(907, 845, 906, 687)); //losePopUp.origin = losePopUp.Center;

            returnButton = new Sprite(UISheet, new Rectangle(1517, 417, 82, 81));
        }
    }
}

