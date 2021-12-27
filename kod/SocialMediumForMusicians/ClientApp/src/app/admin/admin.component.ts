import { Component, OnInit, ViewChild } from "@angular/core";
import { ReportService } from "../report/report.service";
import { Report } from "src/models/report";
import { MatPaginator, PageEvent } from "@angular/material/paginator";
import { Router } from "@angular/router";
import { AuthorizeService } from "src/api-authorization/authorize.service";

@Component({
    selector: 'app-admin-panel',
    templateUrl: './admin.component.html'
})
export class AdminComponent implements OnInit {
    reports: Array<Report>;

    userId: string;

    @ViewChild(MatPaginator)
    private paginator: MatPaginator;
    defaultPageSize = 3;

    constructor(
        private reportService: ReportService,
        private authService: AuthorizeService) {}

    ngOnInit() {
        this.authService.getUser().subscribe(authUser => {
            this.userId = authUser.sub;
        }, err => console.error(err));

        const pageEvent = new PageEvent();
        pageEvent.pageIndex = 0;
        pageEvent.pageSize = this.defaultPageSize;

        this.getReports(pageEvent);
    }

    getReports(event: PageEvent) {
        this.reportService.getReports(this.userId, event.pageIndex, event.pageSize).subscribe(result => {
            this.reports = result.elements;
            this.paginator.pageIndex = result.pageIndex;
            this.paginator.pageSize = result.pageSize;
            this.paginator.length = result.totalCount;
            console.log(this.reports);
        }, err => console.log(err));
    }
}
