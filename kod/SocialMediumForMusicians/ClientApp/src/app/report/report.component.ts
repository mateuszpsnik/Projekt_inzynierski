import { Component, Inject, Input, OnInit } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { FormGroup, FormControl, Validators } from "@angular/forms";
import { Observable } from "rxjs";
import { Report } from "src/models/report";
import { Guid } from 'src/models/guid';
import { ReportService } from "./report.service";

@Component({
    selector: 'app-report-button',
    templateUrl: './report.component.html',
    styleUrls: ['./report.component.css']
})
export class ReportComponent implements OnInit {
    form: FormGroup;
    // if true, show the form with justification textarea
    formVisible = false;

    // Id of the reported user
    @Input() userId: string;

    constructor(
        private service: ReportService) {}

    ngOnInit () {
        this.form = new FormGroup({
            justification: new FormControl('', [ Validators.required,
                Validators.minLength(50), Validators.maxLength(300) ])
        });
    }

    buttonClicked() {
        this.formVisible = !this.formVisible;
    }

    onSubmit() {
        const justification = this.form.get('justification').value;

        if (justification !== '') {
            const report: Report = {
                userId: this.userId,
                justification: justification,
                sentAt: new Date(Date.now())
            };

            this.service.post(report).subscribe(result => {
                console.log(result);
                alert('Zgłoszenie zostało wysłane');
                this.form.reset();
                this.formVisible = false;
            }, err => console.error(err));
        }
    }
}
