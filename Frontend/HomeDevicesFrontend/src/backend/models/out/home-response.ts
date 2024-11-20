export interface HomeResponse {
    id: string;
    name: string;
    location: string;
    latitude: string;
    longitude: string;
    memberCount: number;
    homeOwner: string;
    devices?: any[];
    owner?: any;
    members?: any[];
    rooms?: any[]; 
    memberSettings: any[];
  }
  