import { Component, Inject, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { FormGroup, FormControl } from "@angular/forms";

@Component({
    selector: "app-home-form",
    templateUrl: "./home-form.component.html"
})
export class HomeFormComponent implements OnInit {
    // form model
    form: FormGroup;

    constructor(private router: Router) { }

    ngOnInit() {
        this.form = new FormGroup({
            aaa: new FormControl("")
        });
    }

    onSubmit() {
        let aaaNum = this.form.get("aaa").value;
        this.router.navigate(["/musicians"], { queryParams: { aaa: aaaNum }});
    }
}