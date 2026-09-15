export interface Department {
  readonly name: string;
}

export interface Candidate {
  readonly name: string;
  readonly email: string;
}

export interface Contract {
  readonly candidateName: string;
  readonly email: string;
}

export interface Account {
  readonly name: string;
  readonly login: string;
}
