export interface ApiResponse {
  success: boolean;            
  message: string;            
  validationErrors?: string[]; 
  objectresponse: any;         
}
