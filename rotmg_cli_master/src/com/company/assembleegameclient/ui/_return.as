﻿// Decompiled by AS3 Sorcerer 1.99
// http://www.as3sorcerer.com/

//com.company.assembleegameclient.ui._return

package com.company.assembleegameclient.ui{
    import flash.display.Shape;

    import flash.display.IGraphicsData;
    import com.company.util.GraphicHelper;
    import flash.display.GraphicsSolidFill;
    import flash.display.GraphicsPath;
    import flash.display.GraphicsPathWinding;


    public class _return extends Shape {

        private var _A_f:Vector.<IGraphicsData>;

        private var _7H_:GraphicsSolidFill;
        private var _nM_:GraphicsPath;

        public function _return(_arg1:int, _arg2:uint){
            this._7H_ = new GraphicsSolidFill(0xFFFFFF, 1);
            this._nM_ = new GraphicsPath(new Vector.<int>(), new Vector.<Number>(), GraphicsPathWinding.NON_ZERO);
            this._A_f = new <IGraphicsData>[_7H_, _nM_, GraphicHelper.END_FILL];
            super();
            this._rs(_arg1, _arg2);
        }
        public function _rs(_arg1:int, _arg2:uint):void{
            graphics.clear();
            this._7H_.color = _arg2;
            GraphicHelper._0L_6(this._nM_);
            GraphicHelper.drawSpacer(0, 0, 4, this._nM_);
            GraphicHelper.drawSpacer(_arg1, 0, 4, this._nM_);
            GraphicHelper.drawRect(0, -1, _arg1, 2, this._nM_);
            graphics.drawGraphicsData(this._A_f);
        }

    }
}//package com.company.assembleegameclient.ui

