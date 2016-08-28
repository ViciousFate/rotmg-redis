// Decompiled by AS3 Sorcerer 1.99
// http://www.as3sorcerer.com/

//_F_1.News

package _F_1{
    import flash.display.Sprite;

    import com.company.assembleegameclient.appengine._vt;
    import com.company.assembleegameclient.appengine._0K_R_;


    public class News extends Sprite {

        private var _dL_:Vector.<_0B_m>;

        public function News(_arg1:_0K_R_){
            var _local2:_vt;
            this._dL_ = new Vector.<_0B_m>();
            super();
            for each (_local2 in _arg1._tZ_)
            {
                if (_local2._5U_)
                {
                    this._yQ_(new _0B_m(_local2._5U_, _local2._O_k, _local2._03P_, _local2._qh, _local2._W_e, _arg1.accountId_));
                };
            };
        }
        public function _yQ_(_arg1:_0B_m):void{
            _arg1.y = (4 + (this._dL_.length * (_0B_m.HEIGHT + 4)));
            this._dL_.push(_arg1);
            addChild(_arg1);
        }

    }
}//package _F_1

