import { Injectable } from '@angular/core';
import { TokenInfo } from '../../contracts/authentication/token-info';
import jwt_decode from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class JwtService {
  decodeToken(token: string): TokenInfo | null {
    try {
      const decodedToken = jwt_decode(token);

      return {
        userId: decodedToken.userId,
        email: decodedToken.email,
        name: decodedToken.name,
        primaryCurrency: +decodedToken.primaryCurrency,
        exp: +decodedToken.exp * 1000
      } as TokenInfo;
    } catch {
      return null;
    }
  }
}
