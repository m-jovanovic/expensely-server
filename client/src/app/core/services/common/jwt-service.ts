import { Injectable } from '@angular/core';
import { TokenInfo } from '../../contracts/authentication/token-info';
import jwt_decode from 'jwt-decode';
import { Permission } from '@expensely/core/contracts/authentication/permission.enum';

@Injectable({
  providedIn: 'root'
})
export class JwtService {
  decodeToken(token: string): TokenInfo | null {
    try {
      const decodedToken: any = jwt_decode(token);

      return {
        userId: decodedToken.sub,
        email: decodedToken.email,
        fullName: decodedToken.full_name,
        primaryCurrency: +decodedToken.primary_currency,
        permissions: Array.isArray(decodedToken.permissions)
          ? Array.from(decodedToken.permissions).map((x) => x as Permission)
          : [decodedToken.permissions],
        exp: +decodedToken.exp * 1000
      } as TokenInfo;
    } catch {
      return null;
    }
  }
}
