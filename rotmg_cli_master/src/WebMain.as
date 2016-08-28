// Decompiled by AS3 Sorcerer 1.99
// http://www.as3sorcerer.com/

//WebMain

package {
    import flash.display.Sprite;
    import _0_p._L_y;
    import flash.events.Event;
    import flash.display.StageScaleMode;
    import _U_5._D_c;
    import com.company.assembleegameclient.parameters.Parameters;
    import _G_A_._8P_;
    import _C__._07U_;
    import _T_o._083;
    import _G_A_._F_y;
    import _9u._074;
    import _C_5._tt;
    import _U_._K_a;
    import _R_Q_._0K_S_;
    import _I_j._V_4;
    import _05G_._X_G_;
    import flash.system.Capabilities;

    [SWF(width="800", height="600", frameRate="60", backgroundColor="#000000")]
    public class WebMain extends Sprite {

        protected var context:_L_y;

        public function WebMain(){
            if (stage)
            {
                this.setup();
            } else
            {
                addEventListener(Event.ADDED_TO_STAGE, this.onAddedToStage);
            };
        }
        private function onAddedToStage(_arg1:Event):void{
            removeEventListener(Event.ADDED_TO_STAGE, this.onAddedToStage);
            this.setup();
        }
        private function setup():void{
            this._4y();
            this._i1();
            stage.scaleMode = StageScaleMode.EXACT_FIT;
            var _local1:_D_c = this.context._O_R_.getInstance(_D_c);
            _local1.dispatch();
            this._06p();
        }
        private function _4y():void{
            Parameters.root = stage.root;
        }
        private function _i1():void{
            this.context = new _8P_();
            this.context.extend(_07U_).extend(_083)._K_(_F_y)._K_(_074)._K_(_tt)._K_(_K_a)._K_(_0K_S_)._K_(_V_4)._K_(_X_G_)._K_(this);
        }
        private function _06p():void{
            if (Capabilities.playerType == "Desktop")
            {
                Parameters.data_.fullscreenMode = false;
                Parameters.save();
            };
        }

    }
}//package 

