var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var DentalDrill;
(function (DentalDrill) {
    var CRM;
    (function (CRM) {
        var Pages;
        (function (Pages) {
            var Pickup;
            (function (Pickup) {
                var Index;
                (function (Index) {
                    var BasePickupTab = /** @class */ (function () {
                        function BasePickupTab(form) {
                            this._form = form;
                            this._clientNo = this.getKendoControl("ClientNo", "kendoDropDownList");
                            this._practiceName = this.getControl("PracticeName");
                            this._contactPerson = this.getControl("ContactPerson");
                            this._email = this.getControl("Email");
                            this._phone = this.getControl("Phone");
                            this._addressLine1 = this.getControl("AddressLine1");
                            this._addressLine2 = this.getControl("AddressLine2");
                            this._suburb = this.getControl("Suburb");
                            this._numberOfHandpieces = this.getControl("NumberOfHandpieces");
                        }
                        BasePickupTab.prototype.init = function () {
                            var _this = this;
                            if (this.clientNo) {
                                this.updatePreviousSurgery(this._clientNo.dataItem());
                                this.clientNo.bind("change", function (e) {
                                    var newSurgery = e.sender.dataItem();
                                    if (newSurgery) {
                                        _this.applySurgeryData(newSurgery);
                                    }
                                    _this.updatePreviousSurgery(newSurgery);
                                });
                            }
                            this.numberOfHandpieces.addEventListener("keyup", function (e) {
                                var wrapper = _this.numberOfHandpieces.parentElement;
                                if (_this.numberOfHandpieces.value === "1") {
                                    wrapper.classList.add("pickup-form-field--has-warning");
                                }
                                else {
                                    wrapper.classList.remove("pickup-form-field--has-warning");
                                }
                            });
                        };
                        BasePickupTab.prototype.applySurgeryData = function (value) {
                            this.tryUpdateInputField(this.practiceName, "Name", value.Name);
                            this.tryUpdateInputField(this.contactPerson, "PrincipalDentist", value.PrincipalDentist);
                            this.tryUpdateInputField(this.email, "Email", value.Email);
                            this.tryUpdateInputField(this.phone, "Phone", value.Phone);
                            if (this.tryUpdateInputField(this.addressLine1, "Address", value.Address)) {
                                this.addressLine2.value = "";
                            }
                            this.tryUpdateInputField(this.suburb, "Suburb", value.Suburb);
                        };
                        BasePickupTab.prototype.updatePreviousSurgery = function (value) {
                            this._previousSurgery = value;
                            if (this._previousSurgery && !this._previousSurgery.ClientNo) {
                                this._previousSurgery = undefined;
                            }
                        };
                        BasePickupTab.prototype.tryUpdateInputField = function (field, key, value) {
                            if (field && !this._previousSurgery || this._previousSurgery[key] === field.value) {
                                if (value) {
                                    field.value = value;
                                }
                                else {
                                    field.value = "";
                                }
                                return true;
                            }
                            else {
                                return false;
                            }
                        };
                        BasePickupTab.prototype.tryUpdateSelectField = function (field, key, value, allowedValues) {
                            if (field && !this._previousSurgery || this._previousSurgery[key] === field.value && (!value || allowedValues.indexOf(value) >= 0)) {
                                if (value) {
                                    field.value = value;
                                }
                                else {
                                    field.value = "";
                                }
                                return true;
                            }
                            else {
                                return false;
                            }
                        };
                        BasePickupTab.prototype.getControl = function (name) {
                            return this._form.querySelector(".pickup-form-field[data-field-name='" + name + "']");
                        };
                        BasePickupTab.prototype.getKendoControl = function (name, controlName) {
                            var element = this._form.querySelector(".pickup-form-field[data-field-name='" + name + "']");
                            if (element) {
                                return $(element).data(controlName);
                            }
                            else {
                                return undefined;
                            }
                        };
                        Object.defineProperty(BasePickupTab.prototype, "form", {
                            get: function () {
                                return this._form;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(BasePickupTab.prototype, "clientNo", {
                            get: function () {
                                return this._clientNo;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(BasePickupTab.prototype, "practiceName", {
                            get: function () {
                                return this._practiceName;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(BasePickupTab.prototype, "contactPerson", {
                            get: function () {
                                return this._contactPerson;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(BasePickupTab.prototype, "email", {
                            get: function () {
                                return this._email;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(BasePickupTab.prototype, "phone", {
                            get: function () {
                                return this._phone;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(BasePickupTab.prototype, "addressLine1", {
                            get: function () {
                                return this._addressLine1;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(BasePickupTab.prototype, "addressLine2", {
                            get: function () {
                                return this._addressLine2;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(BasePickupTab.prototype, "suburb", {
                            get: function () {
                                return this._suburb;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(BasePickupTab.prototype, "numberOfHandpieces", {
                            get: function () {
                                return this._numberOfHandpieces;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        return BasePickupTab;
                    }());
                    Index.BasePickupTab = BasePickupTab;
                    var GreaterSydneyPickupTab = /** @class */ (function (_super) {
                        __extends(GreaterSydneyPickupTab, _super);
                        function GreaterSydneyPickupTab(form) {
                            var _this = _super.call(this, form) || this;
                            _this._postcode = _this.getControl("Postcode");
                            return _this;
                        }
                        GreaterSydneyPickupTab.prototype.applySurgeryData = function (value) {
                            _super.prototype.applySurgeryData.call(this, value);
                            this.tryUpdateInputField(this.postcode, "PostCode", value.PostCode);
                        };
                        Object.defineProperty(GreaterSydneyPickupTab.prototype, "postcode", {
                            get: function () {
                                return this._postcode;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        return GreaterSydneyPickupTab;
                    }(BasePickupTab));
                    Index.GreaterSydneyPickupTab = GreaterSydneyPickupTab;
                    var AustraliaPickupTab = /** @class */ (function (_super) {
                        __extends(AustraliaPickupTab, _super);
                        function AustraliaPickupTab(form) {
                            var _this = _super.call(this, form) || this;
                            _this._state = _this.getControl("State");
                            _this._postcode = _this.getControl("Postcode");
                            return _this;
                        }
                        AustraliaPickupTab.prototype.applySurgeryData = function (value) {
                            _super.prototype.applySurgeryData.call(this, value);
                            this.tryUpdateInputField(this.state, "StateName", value.StateName);
                            this.tryUpdateInputField(this.postcode, "PostCode", value.PostCode);
                        };
                        Object.defineProperty(AustraliaPickupTab.prototype, "postcode", {
                            get: function () {
                                return this._postcode;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(AustraliaPickupTab.prototype, "state", {
                            get: function () {
                                return this._state;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        return AustraliaPickupTab;
                    }(BasePickupTab));
                    Index.AustraliaPickupTab = AustraliaPickupTab;
                    var NewZealandPickupTab = /** @class */ (function (_super) {
                        __extends(NewZealandPickupTab, _super);
                        function NewZealandPickupTab(form) {
                            var _this = _super.call(this, form) || this;
                            _this._locality = _this.getControl("Locality");
                            _this._island = _this.getControl("Island");
                            return _this;
                        }
                        NewZealandPickupTab.prototype.applySurgeryData = function (value) {
                            _super.prototype.applySurgeryData.call(this, value);
                            this.tryUpdateInputField(this.locality, "StateName", value.StateName);
                            this.tryUpdateSelectField(this.island, "PostCode", value.PostCode, ["", "NI", "SI"]);
                        };
                        Object.defineProperty(NewZealandPickupTab.prototype, "locality", {
                            get: function () {
                                return this._locality;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(NewZealandPickupTab.prototype, "island", {
                            get: function () {
                                return this._island;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        return NewZealandPickupTab;
                    }(BasePickupTab));
                    Index.NewZealandPickupTab = NewZealandPickupTab;
                    var QueenslandPickupTab = /** @class */ (function (_super) {
                        __extends(QueenslandPickupTab, _super);
                        function QueenslandPickupTab(form) {
                            var _this = _super.call(this, form) || this;
                            _this._postcode = _this.getControl("Postcode");
                            return _this;
                        }
                        QueenslandPickupTab.prototype.applySurgeryData = function (value) {
                            _super.prototype.applySurgeryData.call(this, value);
                            this.tryUpdateInputField(this.postcode, "PostCode", value.PostCode);
                        };
                        Object.defineProperty(QueenslandPickupTab.prototype, "postcode", {
                            get: function () {
                                return this._postcode;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        return QueenslandPickupTab;
                    }(BasePickupTab));
                    Index.QueenslandPickupTab = QueenslandPickupTab;
                    var PickupForm = /** @class */ (function () {
                        function PickupForm(root) {
                            this._greaterSydney = new GreaterSydneyPickupTab(root.querySelector("form.pickup-form__greater-sydney"));
                            this._australia = new AustraliaPickupTab(root.querySelector("form.pickup-form__australia"));
                            this._newZealand = new NewZealandPickupTab(root.querySelector("form.pickup-form__new-zealand"));
                            this._queensland = new QueenslandPickupTab(root.querySelector("form.pickup-form__queensland"));
                        }
                        PickupForm.prototype.init = function () {
                            this._greaterSydney.init();
                            this._australia.init();
                            this._newZealand.init();
                            this._queensland.init();
                        };
                        Object.defineProperty(PickupForm.prototype, "greaterSydney", {
                            get: function () {
                                return this._greaterSydney;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(PickupForm.prototype, "australia", {
                            get: function () {
                                return this._australia;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(PickupForm.prototype, "newZealand", {
                            get: function () {
                                return this._newZealand;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(PickupForm.prototype, "queensland", {
                            get: function () {
                                return this._queensland;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        return PickupForm;
                    }());
                    Index.PickupForm = PickupForm;
                    $(function () {
                        var form = new PickupForm(document.querySelector(".pickup-form"));
                        form.init();
                    });
                })(Index = Pickup.Index || (Pickup.Index = {}));
            })(Pickup = Pages.Pickup || (Pages.Pickup = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=index.js.map