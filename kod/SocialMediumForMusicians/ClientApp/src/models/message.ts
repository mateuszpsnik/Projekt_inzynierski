export interface Message {
    id?: number;
    authorId: string;
    recipentId: string;
    content: string;
    read: boolean;
    sentAt: Date;
}

export interface EmailMessage {
    id?: number;
    authorEmail: string;
    recipentId: string;
    content: string;
    read: boolean;
    sentAt: Date;
}
