import { Inject, Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";
import { Message } from "src/models/message";
import { PaginationApiResult } from "src/models/pagination_api_result";

@Injectable({ providedIn: 'root' })
export class MessageService {
    constructor(
        private http: HttpClient,
        @Inject('BASE_URL')
        private baseUrl: string) {}

    getMessages(userId: string, pageIndex: number, pageSize: number)
            : Observable<PaginationApiResult<Message>> {
                const url = this.baseUrl + 'api/Messages/';
        const params = new HttpParams().set('id', userId)
                                        .set('pageIndex', pageIndex.toString())
                                        .set('pageSize', pageSize.toString());
        return this.http.get<PaginationApiResult<Message>>(url, { params });
    }

    getMessagesThread(userId: string, interId: string)
            : Observable<PaginationApiResult<Message>> {
        const url = this.baseUrl + 'api/Messages/';
        const params = new HttpParams().set('id', userId)
                                       .set('authorId', interId);
        return this.http.get<PaginationApiResult<Message>>(url, { params });
    }

    get(id): Observable<Message> {
        const url = this.baseUrl + 'api/Messages/' + id;
        return this.http.get<Message>(url);
    }

    put(message: Message): Observable<Message> {
        const url = this.baseUrl + 'api/Messages/' + message.id;
        return this.http.put<Message>(url, message);
    }

    post(message: Message): Observable<Message> {
        const url = this.baseUrl + 'api/Messages/';
        return this.http.post<Message>(url, message);
    }
}
