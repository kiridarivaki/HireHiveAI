export class Resume {
    constructor (
        public id: string,
        public fileName: string,
        public fileSize: number,
        public contentType: string,
        public updatedAt: Date, 
        public userId: string 
    ) { }
}