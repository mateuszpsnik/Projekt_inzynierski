import { Inject, Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";
import { Report } from "src/models/report";
import { PaginationApiResult } from "src/models/pagination_api_result";

@Injectable({ providedIn: 'root' })
export class ReportService {
    constructor(
        private http: HttpClient,
        @Inject('BASE_URL')
        private baseUrl: string) {}

    getReports(userId: string, pageIndex: number, pageSize: number)
            : Observable<PaginationApiResult<Report>> {
        const url = this.baseUrl + 'api/Reports';
        const params = new HttpParams().set('pageIndex', pageIndex.toString())
                                       .set('pageSize', pageSize.toString())
                                       .set('userId', userId);
        return this.http.get<PaginationApiResult<Report>>(url, { params });
    }

    post(report: Report): Observable<Report> {
        const url = this.baseUrl + 'api/Reports/';
        return this.http.post<Report>(url, report);
    }
}
