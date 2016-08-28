package mx.core
{
   use namespace mx_internal;
   
   public class RSLData extends Object
   {
      
      public function RSLData(param1:String=null, param2:String=null, param3:String=null, param4:String=null, param5:Boolean=false, param6:Boolean=false, param7:String="default") {
         super();
         this._rslURL = param1;
         this._policyFileURL = param2;
         this._digest = param3;
         this._hashType = param4;
         this._isSigned = param5;
         this._verifyDigest = param6;
         this._applicationDomainTarget = param7;
         this._moduleFactory = this.moduleFactory;
      }
      
      mx_internal  static const VERSION:String = "4.6.0.23201";
      
      private var _applicationDomainTarget:String;
      
      public function get applicationDomainTarget() : String {
         return this._applicationDomainTarget;
      }
      
      private var _digest:String;
      
      public function get digest() : String {
         return this._digest;
      }
      
      private var _hashType:String;
      
      public function get hashType() : String {
         return this._hashType;
      }
      
      private var _isSigned:Boolean;
      
      public function get isSigned() : Boolean {
         return this._isSigned;
      }
      
      private var _moduleFactory:IFlexModuleFactory;
      
      public function get moduleFactory() : IFlexModuleFactory {
         return this._moduleFactory;
      }
      
      public function set moduleFactory(param1:IFlexModuleFactory) : void {
         this._moduleFactory = param1;
      }
      
      private var _policyFileURL:String;
      
      public function get policyFileURL() : String {
         return this._policyFileURL;
      }
      
      private var _rslURL:String;
      
      public function get rslURL() : String {
         return this._rslURL;
      }
      
      private var _verifyDigest:Boolean;
      
      public function get verifyDigest() : Boolean {
         return this._verifyDigest;
      }
   }
}
