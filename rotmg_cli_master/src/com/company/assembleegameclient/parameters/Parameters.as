// Decompiled by AS3 Sorcerer 1.99
// http://www.as3sorcerer.com/

//com.company.assembleegameclient.parameters.Parameters

package com.company.assembleegameclient.parameters{
    import _yn._07Y_;
    
    import com.company.util._0A_s;
    import com.company.util._H_V_;
    
    import flash.display.DisplayObject;
    import flash.net.LocalConnection;
    import flash.net.SharedObject;
    import flash.system.Capabilities;
    import flash.utils.Dictionary;

    public class Parameters {

        public static const isTesting:Boolean = true;
        public static const _0A_G_:String = "123.5.1";
        public static const _wZ_:Boolean = true;
        public static const gamePort:int = 2050;
        public static const _1h:Boolean = false;
        public static const _mg:uint = 10944349; // Possible Guild Colour?
        public static const _8T_:uint = 0xFCDF00;
        public static const _F_g:int = 60;
        public static const RotationSpeed:Number = 0.003; // Rotaion amount per frame
        public static const _E_S_:int = 20;
        public static const SendInfo:String = "";
        public static const SendClient:String = "*Client*";
        public static const SendError:String = "*Error*";
        public static const SendHelp:String = "*Help*";
        public static const SendGuild:String = "*Guild*";
        public static const _0u:int = 1000;
        public static const _0H_m:int = 1000;
        public static const TUT_ID:int = -1;
        public static const NEXUS_ID:int = -2;
        public static const RAND_REALM:int = -3;
        public static const NEXUS_LIMBO:int = -4;
        public static const TEST_ID:int = -6;
        public static const _K_5:Number = 18; // ???
        public static const ToS_Url_:String = "http://www.realmofthemadgod.com/TermsofUse.html";
		public static const musicUrl_:String = "realmofthemadgod.com";
        public static const HelpCommand:String = ((((((((((((("Help:\n" + "[/pause]: pause the game (until you [/pause] again)\n") + "[/who]: list players online\n") + "[/tutorial]: enter the tutorial\n") + "[/yell <message>]: send message to all players in Nexus\n") + "[/tell <player name> <message>]: send a private message to a player\n") + "[/guild <message>]: send a message to your guild\n") + "[/ignore <player name>]: don't show chat messages from player\n") + "[/unignore <player name>]: stop ignoring a player\n") + "[/teleport <player name>]: teleport to a player\n") + "[/trade <player name>]: request a trade with a player\n") + "[/invite <player name>]: invite a player to your guild\n") + "[/join <guild name>]: join a guild (invite necessary)\n") + "[/help]: this message");
        public static const RANDOM1:String = "311f80691451c71b09a13a2a6e";
        public static const RANDOM2:String = "72c5583cafb6818995cbd74b80";
        public static const RSAKey:String = ((((("-----BEGIN PUBLIC KEY-----\n" + "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCqccYHTj4QATbK1m4UVgcTeEYt") + "DZwZxwdayVTFs1jAwKWzoDt/CXhYUX2cEJnA93T4h6a4ysTfUrgBFZ3Nsb4W3/4C") + "rc2TxjOWQOoWQnhzblZEiTcAmIdwdq8AfuZWjRpGhOzHDPK9hcgySrzqD9HJAUsb") + "G2ZzU5zrxHtEPC0nUQIDAQAB\n") + "-----END PUBLIC KEY-----");

        public static var root:DisplayObject;
        public static var data_:Object = null;
        public static var _R_P_:int = 1;
        public static var _Q_b:int = 0;
        public static var _Q_w:Boolean = true;
        public static var _0F_o:Boolean = false;
        public static var _ih:Boolean = false;
        public static var _hk:Boolean = true;
        private static var _Z_U_:SharedObject = null;
        private static var _C_o:Dictionary = new Dictionary();

        public static function _02Q_():String{
            return ((((((Parameters.isTesting) ? "Production" : "Devel") + " Build #") + Parameters._0A_G_) + ((_I_O_()) ? " Testing" : "")));
        }

        public static function _I_O_():Boolean{
            /*var _local2:Object;
            var _local3:LocalConnection;
            var _local1:Boolean;
            if (Capabilities.playerType == "Desktop")
            {
                _local2 = root.loaderInfo.parameters;
                _local1 = !((_local2.deployment == "Production"));
            } else
            {
                _local3 = new LocalConnection();
                _local1 = ((!((_local3.domain == _07Y_._a_(-792, -508)))) && (!((_local3.domain == "realmofthemadgod.appspot.com"))));
            };
            return (_local1);*/
            return false;
        }
        public static function _fK_():String{
            return ("localhost:8888"); // Change this to switch host (Default port 8080)
        }
        public static function _05w():String{
            if (!isTesting)
            {
                return ("UA-99999999-1");
            };
            if (_I_O_())
            {
                return ("UA-11236645-6");
            };
            return ("UA-11236645-4");
        }
        public static function load():void{
            try
            {
                _Z_U_ = SharedObject.getLocal("AssembleeGameClientOptions", "/");
                data_ = _Z_U_.data;
            } catch(error:Error)
            {
                data_ = new Object();
            };
            _fX_();
            save();
        }
        public static function save():void{
            try
            {
                if (_Z_U_ != null)
                {
                    _Z_U_.flush();
                };
            } catch(error:Error)
            {
            };
        }
        private static function _la(_arg1:String, _arg2:uint):void{
            if (!data_.hasOwnProperty(_arg1))
            {
                data_[_arg1] = _arg2;
            };
            _C_o[_arg1] = true;
        }
        public static function setKey(_arg1:String, _arg2:uint):void{
            var _local3:String;
            for (_local3 in _C_o)
            {
                if (data_[_local3] == _arg2)
                {
                    data_[_local3] = _H_V_._0F_r;
                };
            };
            data_[_arg1] = _arg2;
        }
        private static function _iw(_arg1:String, _arg2:*):void{
            if (!data_.hasOwnProperty(_arg1))
            {
                data_[_arg1] = _arg2;
            };
        }
        public static function _fX_():void{
            _la("moveLeft", _H_V_.A);
            _la("moveRight", _H_V_.D);
            _la("moveUp", _H_V_.W);
            _la("moveDown", _H_V_.S);
            _la("rotateLeft", _H_V_.Q);
            _la("rotateRight", _H_V_.E);
            _la("useSpecial", _H_V_.SPACE);
            _la("interact", _H_V_.NUMBER_0);
            _la("useInvSlot1", _H_V_.NUMBER_1);
            _la("useInvSlot2", _H_V_.NUMBER_2);
            _la("useInvSlot3", _H_V_.NUMBER_3);
            _la("useInvSlot4", _H_V_.NUMBER_4);
            _la("useInvSlot5", _H_V_.NUMBER_5);
            _la("useInvSlot6", _H_V_.NUMBER_6);
            _la("useInvSlot7", _H_V_.NUMBER_7);
            _la("useInvSlot8", _H_V_.NUMBER_8);
            _la("escapeToNexus", _H_V_.INSERT);
            _la("escapeToNexus2", _H_V_.F5);
            _la("autofireToggle", _H_V_.I);
            _la("scrollChatUp", _H_V_._R_0);
            _la("scrollChatDown", _H_V_._xs);
            _la("miniMapZoomOut", _H_V_._0F_K_);
            _la("miniMapZoomIn", _H_V_._0E_f);
            _la("resetToDefaultCameraAngle", _H_V_.R);
            _la("togglePerformanceStats", _H_V_._0F_r);
            _la("options", _H_V_.O);
            _la("toggleCentering", _H_V_._0F_r);
            _la("chat", _H_V_.ENTER);
            _la("chatCommand", _H_V_._jE_);
            _la("tell", _H_V_.TAB);
            _la("guildChat", _H_V_.G);
            _la("toggleFullscreen", _H_V_._0F_r);
            _iw("playerObjectType", 782);
            _iw("playMusic", true);
            _iw("playSFX", true);
            _iw("playPewPew", true);
            _iw("centerOnPlayer", true);
            _iw("preferredServer", null);
            _iw("needsTutorial", true);
            _iw("needsNexusTutorial", true);
            _iw("needsRandomRealm", true);
            _iw("cameraAngle", ((7 * Math.PI) / 4));
            _iw("defaultCameraAngle", ((7 * Math.PI) / 4));
            _iw("showQuestPortraits", true);
            _iw("fullscreenMode", false);
            _iw("showProtips", true);
            _iw("protipIndex", 0);
            _iw("joinDate", _0A_s._mP_());
            _iw("lastDailyAnalytics", null);
            _iw("allowRotation", false);
            _iw("charIdUseMap", {});
            _iw("drawShadows", true);
            _iw("textBubbles", true);
            _iw("showTradePopup", true);
            _iw("paymentMethod", null);
            _iw("filterLanguage", true);
            _iw("showGuildInvitePopup", true);
            _iw("showBeginnersOffer", false);
            _iw("beginnersOfferTimeLeft", 0);
            _iw("beginnersOfferShowNow", false);
            _iw("beginnersOfferShowNowTime", 0);
            _iw("watchForTutorialExit", false);
            _iw("contextualClick", true);
            _iw("clickForGold", false);
            if (!data_.hasOwnProperty("needsSurvey"))
            {
                data_.needsSurvey = data_.needsTutorial;
                switch (int((Math.random() * 5)))
                {
                    case 0:
                        data_.surveyDate = 0;
                        data_.playTimeLeftTillSurvey = (5 * 60);
                        data_.surveyGroup = "5MinPlaytime";
                        return;
                    case 1:
                        data_.surveyDate = 0;
                        data_.playTimeLeftTillSurvey = (10 * 60);
                        data_.surveyGroup = "10MinPlaytime";
                        return;
                    case 2:
                        data_.surveyDate = 0;
                        data_.playTimeLeftTillSurvey = (30 * 60);
                        data_.surveyGroup = "30MinPlaytime";
                        return;
                    case 3:
                        data_.surveyDate = (new Date().time + ((((1000 * 60) * 60) * 24) * 7));
                        data_.playTimeLeftTillSurvey = (2 * 60);
                        data_.surveyGroup = "1WeekRealtime";
                        return;
                    case 4:
                        data_.surveyDate = (new Date().time + ((((1000 * 60) * 60) * 24) * 14));
                        data_.playTimeLeftTillSurvey = (2 * 60);
                        data_.surveyGroup = "2WeekRealtime";
                        return;
                };
            };
        }

    }
}//package com.company.assembleegameclient.parameters

