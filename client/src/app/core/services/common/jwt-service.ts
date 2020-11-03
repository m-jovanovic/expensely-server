import { Injectable } from '@angular/core';
import { TokenInfo } from '../../contracts/authentication/token-info';
import jwt_decode from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class JwtService {
  decodeToken(token: string): TokenInfo | null {
    try {
      return jwt_decode(token) as TokenInfo;
    } catch {
      return null;
    }
  }
}
