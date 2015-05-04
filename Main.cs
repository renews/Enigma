using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;

namespace Pyrex
{
    public partial class Main : Form
    {
        private ProcessMemory Mem = new ProcessMemory("PathOfExile");

        private Thread playerStatThread, playerFlaskThread, autoPotionThread;

        private Player _player = new Player();
        private Flasks[] _flasks = new Flasks[5];
        private Buffs[] _playerBuffs = new Buffs[8];

        public Main()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
        }

        private void autoPotion()
        {

        }

        private void getPlayerFlasks()
        {
            while(true)
            {
                try
                {
                    if (Mem.CheckProcess())
                    {
                        Mem.StartProcess();

                        int flaskBase = Mem.ReadInt(Mem.ImageAddress() + 0x008F6A88);

                        for (int x = 0; x < 5; x++)
                        {
                            _flasks[x] = new Flasks();

                            int FlaskInvBase = Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(flaskBase + 0x34) + 0x788) + 0x204) + 0x30) + (x * 4));
                            int Flask_Charge_Ptr = Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(FlaskInvBase) + 0x4) + 0x1C) + 0x4) + 0x4) + 0xC);

                            // Flask Name / Type / Quality

                            _flasks[x].MetaData = Mem.ReadStringUnicode(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(FlaskInvBase) + 0x0) + 0x8), 70);
                            _flasks[x].BaseItemType = Mem.ReadStringUnicode(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(FlaskInvBase) + 0x4) + 0x0) + 0x8) + 0x10), 70);
                            _flasks[x].Quality = Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(FlaskInvBase) + 0x4) + 0x4) + 0xC);

                            // Flask Stats

                            _flasks[x].LocalStat1Type = Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(FlaskInvBase) + 0x4) + 0x18) + 0x20) + 0xC) + 0x0);
                            _flasks[x].LocalStat1Value = Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(FlaskInvBase) + 0x4) + 0x18) + 0x20) + 0xC) + 0x4);

                            _flasks[x].LocalStat2Type = Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(FlaskInvBase) + 0x4) + 0x18) + 0x20) + 0xC) + 0x8);
                            _flasks[x].LocalStat2Value = Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(FlaskInvBase) + 0x4) + 0x18) + 0x20) + 0xC) + 0xC);

                            if (_flasks[x].LocalStat2Type != 13 && _flasks[x].LocalStat2Type != 15 && _flasks[x].LocalStat2Type != 18)
                            {
                                _flasks[x].LocalStat2Type = 0;
                                _flasks[x].LocalStat2Value = 0;
                            }

                            // Flask Prefixes / Suffixes

                            _flasks[x].Prefix = Mem.ReadStringUnicode(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(FlaskInvBase) + 0x4) + 0x1C) + 0x4) + 0x4) + 0x10) + 0x7c) + 0x2c) + 0x30), 74);
                            _flasks[x].PrefixEffect = Mem.ReadStringUnicode(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(FlaskInvBase) + 0x4) + 0x1C) + 0x4) + 0x4) + 0x10) + 0x7c) + 0x2c) + 0x38), 74);

                            if (_flasks[x].Prefix == null || _flasks[x].Prefix == "")
                            {
                                _flasks[x].Prefix = "NULL";
                                _flasks[x].PrefixEffect = "NULL";
                            }

                            _flasks[x].Suffix = Mem.ReadStringUnicode(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(FlaskInvBase) + 0x4) + 0x1C) + 0x4) + 0x4) + 0x10) + 0x7c) + 0x14) + 0x30), 74);
                            _flasks[x].SuffixEffect = Mem.ReadStringUnicode(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(FlaskInvBase) + 0x4) + 0x1C) + 0x4) + 0x4) + 0x10) + 0x7c) + 0x14) + 0x38), 74);

                            // Flask Charges

                            _flasks[x].TotalCharges = Mem.ReadInt(Mem.ReadInt(Flask_Charge_Ptr + 0x8) + 0x8);
                            _flasks[x].CurrentCharges = Mem.ReadInt(Flask_Charge_Ptr + 0xC);
                            _flasks[x].ChargesPerUse = Mem.ReadInt(Mem.ReadInt(Flask_Charge_Ptr + 0x8) + 0xc);

                            if (_flasks[x].SuffixEffect == "FlaskRemovesShock")
                            {
                                _flasks[x].RemovesShock = true;
                            }
                            else
                            {
                                _flasks[x].RemovesShock = false;
                            }

                            if (_flasks[x].SuffixEffect == "FlaskDispellBurning")
                            {
                                _flasks[x].RemovesBurning = true;
                            }
                            else
                            {
                                _flasks[x].RemovesBurning = false;
                            }

                            if (_flasks[x].SuffixEffect == "FlaskDispellChill")
                            {
                                _flasks[x].RemovesFreezing = true;
                            }
                            else
                            {
                                _flasks[x].RemovesFreezing = false;
                            }

                            if (_flasks[x].SuffixEffect == "FlaskRemovesBleeding")
                            {
                                _flasks[x].RemovesBleeding = true;
                            }
                            else
                            {
                                _flasks[x].RemovesBleeding = false;
                            }

                            if (_flasks[x].PrefixEffect == "FlaskRecoverySpeed")
                            {
                                _flasks[x].InstantHealing = true;
                            }
                            else
                            {
                                _flasks[x].InstantHealing = false;
                            }
                        }
                    }

                    if (runDebugCheckBox.Checked)
                    {
                        displayPlayerFlasksDebug();
                    }
                }
                catch { }
            }
        }

        private void getPlayerStats()
        {
            while (true)
            {
                try
                {
                    if (Mem.CheckProcess())
                    {
                        Mem.StartProcess();

                        int playerBase = Mem.ReadInt(Mem.ImageAddress() + 0x0090C2B0); // steam 0x0090E2F8

                        int PlayerStatBase = Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(playerBase + 0x138) + 0x4A4) + 0x4) + 0xC);
                        int PlayerExpBase = Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(Mem.ReadInt(playerBase + 0x138) + 0x2CC) + 0x54) + 0x308);

                        _player.CurrentHealth = Mem.ReadInt(PlayerStatBase + 0x54);
                        _player.MaxHealth = Mem.ReadInt(PlayerStatBase + 0x50);
                        _player.ReservedHealth_Flat = Mem.ReadInt(PlayerStatBase + 0x5C);
                        _player.ReservedHealth_Percent = Mem.ReadInt(PlayerStatBase + 0x60);

                        _player.CurrentMana = Mem.ReadInt(PlayerStatBase + 0x78);
                        _player.MaxMana = Mem.ReadInt(PlayerStatBase + 0x74);
                        _player.ReservedMana_Flat = Mem.ReadInt(PlayerStatBase + 0x80);
                        _player.ReservedMana_Percent = Mem.ReadInt(PlayerStatBase + 0x84);

                        _player.CurrentEnergyShield = Mem.ReadInt(PlayerStatBase + 0x9C);
                        _player.MaxEnergyShield = Mem.ReadInt(PlayerStatBase + 0x98);

                        _player.CurrentEXP = Mem.ReadInt(PlayerExpBase + 0x680);
                        _player.MaxEXP = Mem.ReadInt(PlayerExpBase + 0x684);
                        _player.CurrentLevel = Mem.ReadInt(PlayerExpBase + 0x688);
                        _player.HourEXP = Mem.ReadInt(PlayerExpBase + 0x690);

                        if (_player.HourEXP > 0)
                        {
                            double timeToLevelHours = (double)(_player.MaxEXP - _player.CurrentEXP) / (double)_player.HourEXP;
                            double timeToLevelMinutes = timeToLevelHours * 60;
                            double timeToLevelSeconds = timeToLevelMinutes * 60;

                            TimeSpan t = TimeSpan.FromSeconds(timeToLevelSeconds);

                            //levelTimeLabel.Text = t.ToString(@"hh\:mm\:ss");
                        }

                        Array.Clear(_playerBuffs, 0, _playerBuffs.Length);

                        int buffListStart = Mem.ReadInt(PlayerStatBase + 0xB8);
                        int buffListEnd = Mem.ReadInt(PlayerStatBase + 0xBC);
                        int buffNum = ((buffListEnd - buffListStart) / 4);

                        for(int x = 0; x < buffNum; x++)
                        {
                            _playerBuffs[x] = new Buffs();

                            int buffBasePtr = Mem.ReadInt(Mem.ReadInt(buffListStart + (x * 4)) + 0x4);

                            _playerBuffs[x].Name = Mem.ReadStringUnicode(Mem.ReadInt(Mem.ReadInt(buffBasePtr + 0x4) + 0x0), 70);
                            _playerBuffs[x].Charges = Mem.ReadFloat(buffBasePtr + 0x1C);
                            _playerBuffs[x].Duration = Mem.ReadFloat(buffBasePtr + 0xC);
                        }

                    }

                    if (runDebugCheckBox.Checked)
                    {
                        displayPlayerStatsDebug();
                    }
                }
                catch { }
            }
        }

        private string ToHex(int value)
        {
            return String.Format("0x{0:X}", value);
        }

        private int FromHex(string value)
        {
            // strip the leading 0x
            if (value.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            {
                value = value.Substring(2);
            }
            return Int32.Parse(value, NumberStyles.HexNumber);
        }

        private void displayPlayerStatsDebug()
        {
            #region Player Debug

            levelTextBox.Text = _player.CurrentLevel.ToString();

            maxExpTextBox.Text = _player.MaxEXP.ToString();
            currentExpTextBox.Text = _player.CurrentEXP.ToString();
            expToLevelTextBox.Text = (_player.MaxEXP - _player.CurrentEXP).ToString();

            maxHPTextBox.Text = _player.MaxHealth.ToString();
            currentHPTextBox.Text = _player.CurrentHealth.ToString();
            reservedFlatHPTextBox.Text = _player.ReservedHealth_Flat.ToString();
            reservedPercentHPTextBox.Text = _player.ReservedHealth_Percent.ToString();

            maxManaTextBox.Text = _player.MaxMana.ToString();
            currentManaTextBox.Text = _player.CurrentMana.ToString();
            reservedFlatManaTextBox.Text = _player.ReservedMana_Flat.ToString();
            reservedPercentManaTextBox.Text = _player.ReservedMana_Percent.ToString();

            #endregion

            #region Buff Debug

            if (_playerBuffs[0] != null)
            {
                buff1_chargesTextBox.Text = Math.Ceiling(_playerBuffs[0].Charges).ToString();
                buff1_nameTextBox.Text = _playerBuffs[0].Name;

                if (Math.Ceiling(_playerBuffs[0].Duration).ToString() == "Infinity")
                {
                    buff1_durationTextBox.Text = "Infinite";
                }
                else
                {
                    buff1_durationTextBox.Text = Math.Ceiling(_playerBuffs[0].Duration).ToString();
                }
            }
            else
            {
                buff1_chargesTextBox.Text = "";
                buff1_nameTextBox.Text = "";
                buff1_durationTextBox.Text = "";
            }

            if (_playerBuffs[1] != null)
            {
                buff2_chargesTextBox.Text = Math.Ceiling(_playerBuffs[1].Charges).ToString();
                buff2_nameTextBox.Text = _playerBuffs[1].Name;

                if (Math.Ceiling(_playerBuffs[1].Duration).ToString() == "Infinity")
                {
                    buff2_durationTextBox.Text = "Infinite";
                }
                else
                {
                    buff2_durationTextBox.Text = Math.Ceiling(_playerBuffs[1].Duration).ToString();
                }
            }
            else
            {
                buff2_chargesTextBox.Text = "";
                buff2_nameTextBox.Text = "";
                buff2_durationTextBox.Text = "";
            }

            if (_playerBuffs[2] != null)
            {
                buff3_chargesTextBox.Text = Math.Ceiling(_playerBuffs[2].Charges).ToString();
                buff3_nameTextBox.Text = _playerBuffs[2].Name;

                if (Math.Ceiling(_playerBuffs[2].Duration).ToString() == "Infinity")
                {
                    buff3_durationTextBox.Text = "Infinite";
                }
                else
                {
                    buff3_durationTextBox.Text = Math.Ceiling(_playerBuffs[2].Duration).ToString();
                }
            }
            else
            {
                buff3_chargesTextBox.Text = "";
                buff3_nameTextBox.Text = "";
                buff3_durationTextBox.Text = "";
            }

            if (_playerBuffs[3] != null)
            {
                buff4_chargesTextBox.Text = Math.Ceiling(_playerBuffs[3].Charges).ToString();
                buff4_nameTextBox.Text = _playerBuffs[3].Name;

                if (Math.Ceiling(_playerBuffs[3].Duration).ToString() == "Infinity")
                {
                    buff4_durationTextBox.Text = "Infinite";
                }
                else
                {
                    buff4_durationTextBox.Text = Math.Ceiling(_playerBuffs[3].Duration).ToString();
                }
            }
            else
            {
                buff4_chargesTextBox.Text = "";
                buff4_nameTextBox.Text = "";
                buff4_durationTextBox.Text = "";
            }

            if (_playerBuffs[4] != null)
            {
                buff5_chargesTextBox.Text = Math.Ceiling(_playerBuffs[4].Charges).ToString();
                buff5_nameTextBox.Text = _playerBuffs[4].Name;

                if (Math.Ceiling(_playerBuffs[4].Duration).ToString() == "Infinity")
                {
                    buff5_durationTextBox.Text = "Infinite";
                }
                else
                {
                    buff5_durationTextBox.Text = Math.Ceiling(_playerBuffs[4].Duration).ToString();
                }
            }
            else
            {
                buff5_chargesTextBox.Text = "";
                buff5_nameTextBox.Text = "";
                buff5_durationTextBox.Text = "";
            }

            if (_playerBuffs[5] != null)
            {
                buff6_chargesTextBox.Text = Math.Ceiling(_playerBuffs[5].Charges).ToString();
                buff6_nameTextBox.Text = _playerBuffs[5].Name;

                if (Math.Ceiling(_playerBuffs[5].Duration).ToString() == "Infinity")
                {
                    buff6_durationTextBox.Text = "Infinite";
                }
                else
                {
                    buff6_durationTextBox.Text = Math.Ceiling(_playerBuffs[5].Duration).ToString();
                }
            }
            else
            {
                buff6_chargesTextBox.Text = "";
                buff6_nameTextBox.Text = "";
                buff6_durationTextBox.Text = "";
            }

            if (_playerBuffs[6] != null)
            {
                buff7_chargesTextBox.Text = Math.Ceiling(_playerBuffs[6].Charges).ToString();
                buff7_nameTextBox.Text = _playerBuffs[6].Name;

                if (Math.Ceiling(_playerBuffs[6].Duration).ToString() == "Infinity")
                {
                    buff7_durationTextBox.Text = "Infinite";
                }
                else
                {
                    buff7_durationTextBox.Text = Math.Ceiling(_playerBuffs[6].Duration).ToString();
                }
            }
            else
            {
                buff7_chargesTextBox.Text = "";
                buff7_nameTextBox.Text = "";
                buff7_durationTextBox.Text = "";
            }

            if (_playerBuffs[7] != null)
            {
                buff8_chargesTextBox.Text = Math.Ceiling(_playerBuffs[7].Charges).ToString();
                buff8_nameTextBox.Text = _playerBuffs[7].Name;

                if (Math.Ceiling(_playerBuffs[7].Duration).ToString() == "Infinity")
                {
                    buff8_durationTextBox.Text = "Infinite";
                }
                else
                {
                    buff8_durationTextBox.Text = Math.Ceiling(_playerBuffs[7].Duration).ToString();
                }
            }
            else
            {
                buff8_chargesTextBox.Text = "";
                buff8_nameTextBox.Text = "";
                buff8_durationTextBox.Text = "";
            }

            #endregion
        }

        private void displayPlayerFlasksDebug()
        {
            #region Flask1 Debug

            flask1_metaDataTextBox.Text = _flasks[0].MetaData;
            flask1_baseItemTypeTextBox.Text = _flasks[0].BaseItemType;
            flask1_qualityTextBox.Text = _flasks[0].Quality.ToString();

            flask1_totalChargesTextBox.Text = _flasks[0].TotalCharges.ToString();
            flask1_currentChargesTextBox.Text = _flasks[0].CurrentCharges.ToString();
            flask1_chargesPerUseTextBox.Text = _flasks[0].ChargesPerUse.ToString();

            flask1_localStat1TypeTextBox.Text = _flasks[0].LocalStat1Type.ToString();
            flask1_localStat1ValueTextBox.Text = _flasks[0].LocalStat1Value.ToString();

            flask1_localStat2TypeTextBox.Text = _flasks[0].LocalStat2Type.ToString();
            flask1_localStat2ValueTextBox.Text = _flasks[0].LocalStat2Value.ToString();

            flask1_prefixTextBox.Text = _flasks[0].Prefix;
            flask1_prefixEffectTextBox.Text = _flasks[0].PrefixEffect;

            flask1_suffixTextBox.Text = _flasks[0].Suffix;
            flask1_suffixEffectTextBox.Text = _flasks[0].SuffixEffect;

            flask1_bleedingTextBox.Text = _flasks[0].RemovesBleeding.ToString();
            flask1_burningTextBox.Text = _flasks[0].RemovesBurning.ToString();
            flask1_freezingTextBox.Text = _flasks[0].RemovesFreezing.ToString();
            flask1_shockTextBox.Text = _flasks[0].RemovesShock.ToString();
            flask1_instantlyHealsTextBox.Text = _flasks[0].InstantHealing.ToString();

            #endregion

            //

            #region Flask2 Debug

            flask2_metaDataTextBox.Text = _flasks[1].MetaData;
            flask2_baseItemTypeTextBox.Text = _flasks[1].BaseItemType;
            flask2_qualityTextBox.Text = _flasks[1].Quality.ToString();

            flask2_totalChargesTextBox.Text = _flasks[1].TotalCharges.ToString();
            flask2_currentChargesTextBox.Text = _flasks[1].CurrentCharges.ToString();
            flask2_chargesPerUseTextBox.Text = _flasks[1].ChargesPerUse.ToString();

            flask2_localStat1TypeTextBox.Text = _flasks[1].LocalStat1Type.ToString();
            flask2_localStat1ValueTextBox.Text = _flasks[1].LocalStat1Value.ToString();

            flask2_localStat2TypeTextBox.Text = _flasks[1].LocalStat2Type.ToString();
            flask2_localStat2ValueTextBox.Text = _flasks[1].LocalStat2Value.ToString();

            flask2_prefixTextBox.Text = _flasks[1].Prefix;
            flask2_prefixEffectTextBox.Text = _flasks[1].PrefixEffect;

            flask2_suffixTextBox.Text = _flasks[1].Suffix;
            flask2_suffixEffectTextBox.Text = _flasks[1].SuffixEffect;

            flask2_bleedingTextBox.Text = _flasks[1].RemovesBleeding.ToString();
            flask2_burningTextBox.Text = _flasks[1].RemovesBurning.ToString();
            flask2_freezingTextBox.Text = _flasks[1].RemovesFreezing.ToString();
            flask2_shockTextBox.Text = _flasks[1].RemovesShock.ToString();
            flask2_instantlyHealsTextBox.Text = _flasks[1].InstantHealing.ToString();

            #endregion

            //

            #region Flask3 Debug

            flask3_metaDataTextBox.Text = _flasks[2].MetaData;
            flask3_baseItemTypeTextBox.Text = _flasks[2].BaseItemType;
            flask3_qualityTextBox.Text = _flasks[2].Quality.ToString();

            flask3_totalChargesTextBox.Text = _flasks[2].TotalCharges.ToString();
            flask3_currentChargesTextBox.Text = _flasks[2].CurrentCharges.ToString();
            flask3_chargesPerUseTextBox.Text = _flasks[2].ChargesPerUse.ToString();

            flask3_localStat1TypeTextBox.Text = _flasks[2].LocalStat1Type.ToString();
            flask3_localStat1ValueTextBox.Text = _flasks[2].LocalStat1Value.ToString();

            flask3_localStat2TypeTextBox.Text = _flasks[2].LocalStat2Type.ToString();
            flask3_localStat2ValueTextBox.Text = _flasks[2].LocalStat2Value.ToString();

            flask3_prefixTextBox.Text = _flasks[2].Prefix;
            flask3_prefixEffectTextBox.Text = _flasks[2].PrefixEffect;

            flask3_suffixTextBox.Text = _flasks[2].Suffix;
            flask3_suffixEffectTextBox.Text = _flasks[2].SuffixEffect;

            flask3_bleedingTextBox.Text = _flasks[2].RemovesBleeding.ToString();
            flask3_burningTextBox.Text = _flasks[2].RemovesBurning.ToString();
            flask3_freezingTextBox.Text = _flasks[2].RemovesFreezing.ToString();
            flask3_shockTextBox.Text = _flasks[2].RemovesShock.ToString();
            flask3_instantlyHealsTextBox.Text = _flasks[2].InstantHealing.ToString();

            #endregion

            //

            #region Flask4 Debug

            flask4_metaDataTextBox.Text = _flasks[3].MetaData;
            flask4_baseItemTypeTextBox.Text = _flasks[3].BaseItemType;
            flask4_qualityTextBox.Text = _flasks[3].Quality.ToString();

            flask4_totalChargesTextBox.Text = _flasks[3].TotalCharges.ToString();
            flask4_currentChargesTextBox.Text = _flasks[3].CurrentCharges.ToString();
            flask4_chargesPerUseTextBox.Text = _flasks[3].ChargesPerUse.ToString();

            flask4_localStat1TypeTextBox.Text = _flasks[3].LocalStat1Type.ToString();
            flask4_localStat1ValueTextBox.Text = _flasks[2].LocalStat1Value.ToString();

            flask4_localStat2TypeTextBox.Text = _flasks[3].LocalStat2Type.ToString();
            flask4_localStat2ValueTextBox.Text = _flasks[3].LocalStat2Value.ToString();

            flask4_prefixTextBox.Text = _flasks[3].Prefix;
            flask4_prefixEffectTextBox.Text = _flasks[3].PrefixEffect;

            flask4_suffixTextBox.Text = _flasks[3].Suffix;
            flask4_suffixEffectTextBox.Text = _flasks[3].SuffixEffect;

            flask4_bleedingTextBox.Text = _flasks[3].RemovesBleeding.ToString();
            flask4_burningTextBox.Text = _flasks[3].RemovesBurning.ToString();
            flask4_freezingTextBox.Text = _flasks[3].RemovesFreezing.ToString();
            flask4_shockTextBox.Text = _flasks[3].RemovesShock.ToString();
            flask4_instantlyHealsTextBox.Text = _flasks[3].InstantHealing.ToString();

            #endregion

            //

            #region Flask5 Debug

            flask5_metaDataTextBox.Text = _flasks[4].MetaData;
            flask5_baseItemTypeTextBox.Text = _flasks[4].BaseItemType;
            flask5_qualityTextBox.Text = _flasks[4].Quality.ToString();

            flask5_totalChargesTextBox.Text = _flasks[4].TotalCharges.ToString();
            flask5_currentChargesTextBox.Text = _flasks[4].CurrentCharges.ToString();
            flask5_chargesPerUseTextBox.Text = _flasks[4].ChargesPerUse.ToString();

            flask5_localStat1TypeTextBox.Text = _flasks[4].LocalStat1Type.ToString();
            flask5_localStat1ValueTextBox.Text = _flasks[4].LocalStat1Value.ToString();

            flask5_localStat2TypeTextBox.Text = _flasks[4].LocalStat2Type.ToString();
            flask5_localStat2ValueTextBox.Text = _flasks[4].LocalStat2Value.ToString();

            flask5_prefixTextBox.Text = _flasks[4].Prefix;
            flask5_prefixEffectTextBox.Text = _flasks[4].PrefixEffect;

            flask5_suffixTextBox.Text = _flasks[4].Suffix;
            flask5_suffixEffectTextBox.Text = _flasks[4].SuffixEffect;

            flask5_bleedingTextBox.Text = _flasks[4].RemovesBleeding.ToString();
            flask5_burningTextBox.Text = _flasks[4].RemovesBurning.ToString();
            flask5_freezingTextBox.Text = _flasks[4].RemovesFreezing.ToString();
            flask5_shockTextBox.Text = _flasks[4].RemovesShock.ToString();
            flask5_instantlyHealsTextBox.Text = _flasks[4].InstantHealing.ToString();

            #endregion
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                playerStatThread = new Thread(new ThreadStart(getPlayerStats));
                playerStatThread.Start();

                playerFlaskThread = new Thread(new ThreadStart(getPlayerFlasks));
                playerFlaskThread.Start();

                autoPotionThread = new Thread(new ThreadStart(autoPotion));
                autoPotionThread.Start();
            }
            catch { }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                playerFlaskThread.Abort();
                playerFlaskThread.Join();

                playerStatThread.Abort();
                playerStatThread.Join();

                //debugThread.Abort();
                //debugThread.Join();

            }
            catch { }
        }
    }
}
