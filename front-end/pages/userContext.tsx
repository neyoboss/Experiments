import { ReactNode, createContext, useContext, useState } from 'react';


interface IUserContext {
    user: any;
    setUser: (user: any) => void;
  }
export const UserContext = createContext<IUserContext|undefined>(undefined);

interface UserProviderProps{
    children:ReactNode;
}

export const UserProvider: React.FC<UserProviderProps> = ({ children }) => {
    const [user, setUser] = useState<any>(null);
  
    return (
      <UserContext.Provider value={{ user, setUser }}>
        {children}
      </UserContext.Provider>
    );
  };

export default function nothinf(){
  return(
    <>
    </>
    )
}
