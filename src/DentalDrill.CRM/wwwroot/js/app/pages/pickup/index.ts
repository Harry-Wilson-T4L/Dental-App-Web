namespace DentalDrill.CRM.Pages.Pickup.Index {
    export interface PickupSurgery {
        ClientNo: string;
        FullName: string;
        Name: string;
        Address: string;
        PrincipalDentist: string;
        Suburb: string;
        PostCode: string;
        StateName: string;
        Email: string;
        Phone: string;
    }

    export abstract class BasePickupTab {
        private readonly _form: HTMLFormElement;
        private readonly _clientNo: kendo.ui.DropDownList;
        private readonly _practiceName: HTMLInputElement;
        private readonly _contactPerson: HTMLInputElement;
        private readonly _email: HTMLInputElement;
        private readonly _phone: HTMLInputElement;
        private readonly _addressLine1: HTMLInputElement;
        private readonly _addressLine2: HTMLInputElement;
        private readonly _suburb: HTMLInputElement;
        private readonly _numberOfHandpieces: HTMLInputElement;
        
        private _previousSurgery: PickupSurgery;

        constructor(form: HTMLFormElement) {
            this._form = form;

            this._clientNo = this.getKendoControl<kendo.ui.DropDownList>("ClientNo", "kendoDropDownList");
            this._practiceName = this.getControl<HTMLInputElement>("PracticeName");
            this._contactPerson = this.getControl<HTMLInputElement>("ContactPerson");
            this._email = this.getControl<HTMLInputElement>("Email");
            this._phone = this.getControl<HTMLInputElement>("Phone");
            this._addressLine1 = this.getControl<HTMLInputElement>("AddressLine1");
            this._addressLine2 = this.getControl<HTMLInputElement>("AddressLine2");
            this._suburb = this.getControl<HTMLInputElement>("Suburb");
            this._numberOfHandpieces = this.getControl<HTMLInputElement>("NumberOfHandpieces");
        }

        init() {
            if (this.clientNo) {
                this.updatePreviousSurgery(this._clientNo.dataItem());
                this.clientNo.bind("change", (e: kendo.ui.DropDownListChangeEvent) => {
                    const newSurgery = e.sender.dataItem() as PickupSurgery;
                    if (newSurgery) {
                        this.applySurgeryData(newSurgery);
                    }

                    this.updatePreviousSurgery(newSurgery);
                });
            }

            this.numberOfHandpieces.addEventListener("keyup", e => {
                const wrapper = this.numberOfHandpieces.parentElement;
                if (this.numberOfHandpieces.value === "1") {
                    wrapper.classList.add("pickup-form-field--has-warning");
                } else {
                    wrapper.classList.remove("pickup-form-field--has-warning");
                }
            });
        }

        protected applySurgeryData(value: PickupSurgery): void {
            this.tryUpdateInputField(this.practiceName, "Name", value.Name);
            this.tryUpdateInputField(this.contactPerson, "PrincipalDentist", value.PrincipalDentist);
            this.tryUpdateInputField(this.email, "Email", value.Email);
            this.tryUpdateInputField(this.phone, "Phone", value.Phone);
            if (this.tryUpdateInputField(this.addressLine1, "Address", value.Address)) {
                this.addressLine2.value = "";
            }
            this.tryUpdateInputField(this.suburb, "Suburb", value.Suburb);
        }

        protected updatePreviousSurgery(value: PickupSurgery): void {
            this._previousSurgery = value;
            if (this._previousSurgery && !this._previousSurgery.ClientNo) {
                this._previousSurgery = undefined;
            }
        }

        protected tryUpdateInputField(field: HTMLInputElement, key: string, value: string): boolean {
            if (field && !this._previousSurgery || this._previousSurgery[key] === field.value) {
                if (value) {
                    field.value = value;
                } else {
                    field.value = "";
                }
                return true;
            } else {
                return false;
            }
        }

        protected tryUpdateSelectField(field: HTMLSelectElement, key: string, value: string, allowedValues: string[]): boolean {
            if (field && !this._previousSurgery || this._previousSurgery[key] === field.value && (!value || allowedValues.indexOf(value) >= 0)) {
                if (value) {
                    field.value = value;
                } else {
                    field.value = "";
                }
                return true;
            } else {
                return false;
            }
        }

        protected getControl<T extends HTMLElement>(name: string) : T {
            return this._form.querySelector(`.pickup-form-field[data-field-name='${name}']`) as T;
        }

        protected getKendoControl<T extends kendo.ui.Widget>(name: string, controlName: string): T {
            const element = this._form.querySelector(`.pickup-form-field[data-field-name='${name}']`);
            if (element) {
                return $(element).data(controlName);
            } else {
                return undefined;
            }
        }

        protected get form(): HTMLFormElement {
            return this._form;
        }

        get clientNo(): kendo.ui.DropDownList {
            return this._clientNo;
        }

        get practiceName(): HTMLInputElement {
            return this._practiceName;
        }

        get contactPerson(): HTMLInputElement {
            return this._contactPerson;
        }

        get email(): HTMLInputElement {
            return this._email;
        }

        get phone(): HTMLInputElement {
            return this._phone;
        }

        get addressLine1(): HTMLInputElement {
            return this._addressLine1;
        }

        get addressLine2(): HTMLInputElement {
            return this._addressLine2;
        }

        get suburb(): HTMLInputElement {
            return this._suburb;
        }

        get numberOfHandpieces(): HTMLInputElement {
            return this._numberOfHandpieces;
        }
    }

    export class GreaterSydneyPickupTab extends BasePickupTab {
        private readonly _postcode: HTMLInputElement;

        constructor(form: HTMLFormElement) {
            super(form);
            this._postcode = this.getControl<HTMLInputElement>("Postcode");
        }

        protected applySurgeryData(value: PickupSurgery): void {
            super.applySurgeryData(value);
            this.tryUpdateInputField(this.postcode, "PostCode", value.PostCode);
        }

        get postcode(): HTMLInputElement {
            return this._postcode;
        }
    }

    export class AustraliaPickupTab extends BasePickupTab {
        private readonly _state: HTMLInputElement;
        private readonly _postcode: HTMLInputElement;

        constructor(form: HTMLFormElement) {
            super(form);
            this._state = this.getControl<HTMLInputElement>("State");
            this._postcode = this.getControl<HTMLInputElement>("Postcode");
        }

        protected applySurgeryData(value: PickupSurgery): void {
            super.applySurgeryData(value);
            this.tryUpdateInputField(this.state, "StateName", value.StateName);
            this.tryUpdateInputField(this.postcode, "PostCode", value.PostCode);
        }

        get postcode(): HTMLInputElement {
            return this._postcode;
        }

        get state(): HTMLInputElement {
            return this._state;
        }
    }

    export class NewZealandPickupTab extends BasePickupTab {
        private readonly _locality: HTMLInputElement;
        private readonly _island: HTMLSelectElement;

        constructor(form: HTMLFormElement) {
            super(form);
            this._locality = this.getControl<HTMLInputElement>("Locality");
            this._island = this.getControl<HTMLSelectElement>("Island");
        }

        protected applySurgeryData(value: PickupSurgery): void {
            super.applySurgeryData(value);
            this.tryUpdateInputField(this.locality, "StateName", value.StateName);
            this.tryUpdateSelectField(this.island, "PostCode", value.PostCode, [ "", "NI", "SI" ]);
        }

        get locality(): HTMLInputElement {
            return this._locality;
        }

        get island(): HTMLSelectElement {
            return this._island;
        }
    }

    export class QueenslandPickupTab extends BasePickupTab {
        private readonly _postcode: HTMLInputElement;

        constructor(form: HTMLFormElement) {
            super(form);
            this._postcode = this.getControl<HTMLInputElement>("Postcode");
        }

        protected applySurgeryData(value: PickupSurgery): void {
            super.applySurgeryData(value);
            this.tryUpdateInputField(this.postcode, "PostCode", value.PostCode);
        }

        get postcode(): HTMLInputElement {
            return this._postcode;
        }
    }

    export class PickupForm {
        private _greaterSydney: GreaterSydneyPickupTab;
        private _australia: AustraliaPickupTab;
        private _newZealand: NewZealandPickupTab;
        private _queensland: QueenslandPickupTab;

        constructor(root: HTMLElement) {
            this._greaterSydney = new GreaterSydneyPickupTab(root.querySelector("form.pickup-form__greater-sydney"));
            this._australia = new AustraliaPickupTab(root.querySelector("form.pickup-form__australia"));
            this._newZealand = new NewZealandPickupTab(root.querySelector("form.pickup-form__new-zealand"));
            this._queensland = new QueenslandPickupTab(root.querySelector("form.pickup-form__queensland"))
        }

        init() {
            this._greaterSydney.init();
            this._australia.init();
            this._newZealand.init();
            this._queensland.init();
        }

        get greaterSydney(): GreaterSydneyPickupTab {
            return this._greaterSydney;
        }

        get australia(): AustraliaPickupTab {
            return this._australia;
        }

        get newZealand(): NewZealandPickupTab {
            return this._newZealand;
        }

        get queensland(): QueenslandPickupTab {
            return this._queensland;
        }
    }

    $(() => {
        const form = new PickupForm(document.querySelector(".pickup-form"));
        form.init();
    });
}