import { Component, OnInit } from "@angular/core";
import { FormGroup, FormControl } from "@angular/forms";

@Component({
    selector: "signup-widget",
    templateUrl: "./signup-widget.component.html"
})
export class SignupWidgetComponent implements OnInit {
    form: FormGroup;
    email: string;

    constructor() { }
    
    ngOnInit() {
        this.form = new FormGroup({
            email: new FormControl("")
        });
    }

    onSubmit() {
        if (this.form.get("email").value != "") {
            this.email = this.form.get("email").value;
            console.log(this.email);
        }
    }
}