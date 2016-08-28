package mx.formatters
{
   import mx.core.mx_internal;
   
   use namespace mx_internal;
   
   public class DateFormatter extends Formatter
   {
      
      public function DateFormatter() {
         super();
      }
      
      mx_internal  static const VERSION:String = "4.6.0.23201";
      
      private static const VALID_PATTERN_CHARS:String = "Y,M,D,A,E,H,J,K,L,N,S,Q";
      
      public static function parseDateString(param1:String) : Date {
         var _loc14_:String = null;
         var _loc15_:* = 0;
         var _loc16_:* = 0;
         var _loc17_:String = null;
         var _loc18_:String = null;
         var _loc19_:* = 0;
         if(!param1 || param1 == "")
         {
            return null;
         }
         var _loc2_:* = -1;
         var _loc3_:* = -1;
         var _loc4_:* = -1;
         var _loc5_:* = -1;
         var _loc6_:* = -1;
         var _loc7_:* = -1;
         var _loc8_:* = "";
         var _loc9_:Object = 0;
         var _loc10_:* = 0;
         var _loc11_:int = param1.length;
         var _loc12_:RegExp = new RegExp("(GMT|UTC)((\\+|-)\\d\\d\\d\\d )?","ig");
         var param1:String = param1.replace(_loc12_,"");
         while(_loc10_ < _loc11_)
         {
            _loc8_ = param1.charAt(_loc10_);
            _loc10_++;
            if(!(_loc8_ <= " " || _loc8_ == ","))
            {
               if(_loc8_ == "/" || _loc8_ == ":" || _loc8_ == "+" || _loc8_ == "-")
               {
                  _loc9_ = _loc8_;
               }
               else
               {
                  if("a" <= _loc8_ && _loc8_ <= "z" || "A" <= _loc8_ && _loc8_ <= "Z")
                  {
                     _loc14_ = _loc8_;
                     while(_loc10_ < _loc11_)
                     {
                        _loc8_ = param1.charAt(_loc10_);
                        if(!("a" <= _loc8_ && _loc8_ <= "z" || "A" <= _loc8_ && _loc8_ <= "Z"))
                        {
                           break;
                        }
                        _loc14_ = _loc14_ + _loc8_;
                        _loc10_++;
                     }
                     _loc15_ = DateBase.defaultStringKey.length;
                     _loc16_ = 0;
                     while(_loc16_ < _loc15_)
                     {
                        _loc17_ = String(DateBase.defaultStringKey[_loc16_]);
                        if(_loc17_.toLowerCase() == _loc14_.toLowerCase() || _loc17_.toLowerCase().substr(0,3) == _loc14_.toLowerCase())
                        {
                           if(_loc16_ == 13)
                           {
                              if(_loc5_ > 12 || _loc5_ < 1)
                              {
                                 break;
                              }
                              if(_loc5_ < 12)
                              {
                                 _loc5_ = _loc5_ + 12;
                              }
                           }
                           else
                           {
                              if(_loc16_ == 12)
                              {
                                 if(_loc5_ > 12 || _loc5_ < 1)
                                 {
                                    break;
                                 }
                                 if(_loc5_ == 12)
                                 {
                                    _loc5_ = 0;
                                 }
                              }
                              else
                              {
                                 if(_loc16_ < 12)
                                 {
                                    if(_loc3_ < 0)
                                    {
                                       _loc3_ = _loc16_;
                                    }
                                    else
                                    {
                                       break;
                                    }
                                 }
                              }
                           }
                           break;
                        }
                        _loc16_++;
                     }
                     _loc9_ = 0;
                  }
                  else
                  {
                     if("0" <= _loc8_ && _loc8_ <= "9")
                     {
                        _loc18_ = _loc8_;
                        while("0" <= (_loc8_ = param1.charAt(_loc10_)) && _loc8_ <= "9" && _loc10_ < _loc11_)
                        {
                           _loc18_ = _loc18_ + _loc8_;
                           _loc10_++;
                        }
                        _loc19_ = int(_loc18_);
                        if(_loc19_ >= 70)
                        {
                           if(_loc2_ != -1)
                           {
                              break;
                           }
                           if(_loc8_ <= " " || _loc8_ == "," || _loc8_ == "." || _loc8_ == "/" || _loc8_ == "-" || _loc10_ >= _loc11_)
                           {
                              _loc2_ = _loc19_;
                           }
                           else
                           {
                              break;
                           }
                        }
                        else
                        {
                           if(_loc8_ == "/" || _loc8_ == "-" || _loc8_ == ".")
                           {
                              if(_loc3_ < 0)
                              {
                                 _loc3_ = _loc19_-1;
                              }
                              else
                              {
                                 if(_loc4_ < 0)
                                 {
                                    _loc4_ = _loc19_;
                                 }
                                 else
                                 {
                                    break;
                                 }
                              }
                           }
                           else
                           {
                              if(_loc8_ == ":")
                              {
                                 if(_loc5_ < 0)
                                 {
                                    _loc5_ = _loc19_;
                                 }
                                 else
                                 {
                                    if(_loc6_ < 0)
                                    {
                                       _loc6_ = _loc19_;
                                    }
                                    else
                                    {
                                       break;
                                    }
                                 }
                              }
                              else
                              {
                                 if(_loc5_ >= 0 && _loc6_ < 0)
                                 {
                                    _loc6_ = _loc19_;
                                 }
                                 else
                                 {
                                    if(_loc6_ >= 0 && _loc7_ < 0)
                                    {
                                       _loc7_ = _loc19_;
                                    }
                                    else
                                    {
                                       if(_loc4_ < 0)
                                       {
                                          _loc4_ = _loc19_;
                                       }
                                       else
                                       {
                                          if(_loc2_ < 0 && _loc3_ >= 0 && _loc4_ >= 0)
                                          {
                                             _loc2_ = 2000 + _loc19_;
                                          }
                                          else
                                          {
                                             break;
                                          }
                                       }
                                    }
                                 }
                              }
                           }
                        }
                        _loc9_ = 0;
                     }
                  }
               }
            }
         }
         if(_loc2_ < 0 || _loc3_ < 0 || _loc3_ > 11 || _loc4_ < 1 || _loc4_ > 31)
         {
            return null;
         }
         if(_loc7_ < 0)
         {
            _loc7_ = 0;
         }
         if(_loc6_ < 0)
         {
            _loc6_ = 0;
         }
         if(_loc5_ < 0)
         {
            _loc5_ = 0;
         }
         var _loc13_:Date = new Date(_loc2_,_loc3_,_loc4_,_loc5_,_loc6_,_loc7_);
         if(!(_loc4_ == _loc13_.getDate()) || !(_loc3_ == _loc13_.getMonth()))
         {
            return null;
         }
         return _loc13_;
      }
      
      private var _formatString:String;
      
      private var formatStringOverride:String;
      
      public function get formatString() : String {
         return this._formatString;
      }
      
      public function set formatString(param1:String) : void {
         this.formatStringOverride = param1;
         this._formatString = param1 != null?param1:resourceManager.getString("SharedResources","dateFormat");
      }
      
      override protected function resourcesChanged() : void {
         super.resourcesChanged();
         this.formatString = this.formatStringOverride;
      }
      
      override public function format(param1:Object) : String {
         var _loc2_:String = null;
         if(error)
         {
            error = null;
         }
         if(!param1 || param1 is String && param1 == "")
         {
            error = defaultInvalidValueError;
            return "";
         }
         if(param1 is String)
         {
            param1 = DateFormatter.parseDateString(String(param1));
            if(!param1)
            {
               error = defaultInvalidValueError;
               return "";
            }
         }
         else
         {
            if(!(param1 is Date))
            {
               error = defaultInvalidValueError;
               return "";
            }
         }
         var _loc3_:* = 0;
         var _loc4_:* = "";
         var _loc5_:int = this.formatString.length;
         var _loc6_:* = 0;
         while(_loc6_ < _loc5_)
         {
            _loc2_ = this.formatString.charAt(_loc6_);
            if(!(VALID_PATTERN_CHARS.indexOf(_loc2_) == -1) && !(_loc2_ == ","))
            {
               _loc3_++;
               if(_loc4_.indexOf(_loc2_) == -1)
               {
                  _loc4_ = _loc4_ + _loc2_;
               }
               else
               {
                  if(_loc2_ != this.formatString.charAt(Math.max(_loc6_-1,0)))
                  {
                     error = defaultInvalidFormatError;
                     return "";
                  }
               }
            }
            _loc6_++;
         }
         if(_loc3_ < 1)
         {
            error = defaultInvalidFormatError;
            return "";
         }
         var _loc7_:StringFormatter = new StringFormatter(this.formatString,VALID_PATTERN_CHARS,DateBase.extractTokenDate);
         return _loc7_.formatValue(param1);
      }
   }
}
