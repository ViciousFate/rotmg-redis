// Decompiled by AS3 Sorcerer 1.99
// http://www.as3sorcerer.com/

//com.company.assembleegameclient.net._W_v

package com.company.assembleegameclient.net{
    import com.company.assembleegameclient.util.Currency;

    class _W_v {

        private var id_:String;
        private var price_:int;
        private var currency_:int;
        private var converted_:Boolean;

        public function _W_v(_arg1:String, _arg2:int, _arg3:int, _arg4:Boolean){
            this.id_ = _arg1;
            this.price_ = _arg2;
            this.currency_ = _arg3;
            this.converted_ = _arg4;
        }
        public function _06m():void{
            switch (this.currency_)
            {
                case Currency._class:
                    return;
                case Currency.FAME:
                    return;
                case Currency._A_h:
                    return;
            };
        }

    }
}//package com.company.assembleegameclient.net

