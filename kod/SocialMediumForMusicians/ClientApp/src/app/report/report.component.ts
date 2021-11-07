import { Component, Inject, Input, OnInit } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { FormGroup, FormControl } from "@angular/forms";
import { Observable } from "rxjs";
import { Report } from "src/models/report";
import { Guid } from 'src/models/guid';

@Component({
    selector: 'app-report-button',
    templateUrl: './report.component.html'
})
export class ReportComponent implements OnInit {
    form: FormGroup;
    // if true, show the form with justification textarea
    formVisible = false;

    // Id of the reported user
    @Input() userId: string;

    constructor(
        private http: HttpClient,
        @Inject('BASE_URL')
        private baseUrl: string) {}

    ngOnInit () {
        this.form = new FormGroup({
            justification: new FormControl('')
        });
    }

    buttonClicked() {
        this.formVisible = !this.formVisible;
    }

    onSubmit() {
        const justification = this.form.get('justification').value;
        const url = this.baseUrl + 'api/Reports/';

        if (justification !== '') {
            const report: Report = {
                id: Guid.newGuid(),
                userId: this.userId,
                justification: justification
            };

            this.http.post<Report>(url, report).subscribe(result => {
                console.log(result);
                alert('Zgłoszenie zostało wysłane');
            }, err => console.error(err));
        }
    }
}
