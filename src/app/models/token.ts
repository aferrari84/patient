export class Token {
    constructor(
        public token: string,
        public authenticated: boolean,
        public tokenExpires: string
    ) { }
}