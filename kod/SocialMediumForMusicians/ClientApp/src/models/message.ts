export interface Message {
    id?: string;
    authorId: string;
    recipentId: string;
    content: string;
    read: boolean;
    sentAt: Date;
}

export interface EmailMessage {
    id?: string;
    authorEmail: string;
    recipentId: string;
    content: string;
    read: boolean;
    sentAt: Date;
}
