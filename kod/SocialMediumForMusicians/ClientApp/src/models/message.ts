export interface Message {
    id?: string;
    authorId: string;
    recipentId: string;
    content: string;
    read: boolean;
    sentAt: Date;
    authorName?: string;
    authorImgFilename?: string;
}

export interface EmailMessage {
    id?: string;
    authorEmail: string;
    recipentId: string;
    content: string;
    read: boolean;
    sentAt: Date;
}
