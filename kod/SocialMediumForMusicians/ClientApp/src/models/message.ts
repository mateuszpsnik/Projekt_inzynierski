export interface Message {
    id: number;
    authorId: string;
    recipentId: string;
    content: string;
    read: boolean;
    sentAt: Date;
}
