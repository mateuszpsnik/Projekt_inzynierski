import { Inject, Injectable } from "@angular/core";
import { HttpClient, HttpParams} from "@angular/common/http";
import { Observable } from "rxjs";
import { Review } from "../../models/review";
import { PaginationApiResult } from 'src/models/pagination_api_result';

@Injectable({ providedIn: "root" })
export class ReviewService {
    constructor(
        private http: HttpClient,
        @Inject("BASE_URL")
        private baseUrl: string) { }

    getReviews(top: number) : Observable<PaginationApiResult<Review>> {
        let url = this.baseUrl + "api/Reviews/";
        let params = new HttpParams().set("top", top.toString());

        return this.http.get<PaginationApiResult<Review>>(url, { params });
    }

    getReviewsList(id: string, pageIndex: number, pageSize: number)
            : Observable<PaginationApiResult<Review>> {
        let url = this.baseUrl + "api/Reviews/";
        let params = new HttpParams().set("id", id.toString())
                                     .set("pageIndex", pageIndex.toString())
                                     .set("pageSize", pageSize.toString());
        return this.http.get<PaginationApiResult<Review>>(url, { params });
    }

    get(id): Observable<Review> {
        let url = this.baseUrl + "api/Reviews/" + id;
        return this.http.get<Review>(url);
    }

    put(review): Observable<Review> {
        let url = this.baseUrl + "api/Reviews/" + review.id;
        return this.http.put<Review>(url, review);
    }

    post(review: Review): Observable<Review> {
        let url = this.baseUrl + "api/Reviews/";
        return this.http.post<Review>(url, review);
    }
}