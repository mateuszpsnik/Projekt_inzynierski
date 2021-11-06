import { Inject, Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";
import { Message } from "src/models/message";

@Injectable({ providedIn: 'root' })
export class MessageService {
    constructor(
        private http: HttpClient,
        @Inject('BASE_URL')
        private baseUrl: string) {}

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
