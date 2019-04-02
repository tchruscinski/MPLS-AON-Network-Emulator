To jest szkielet serwera UDP. A w³aœciwie nie serwera, a socketu. Zrobi³em to opieraj¹c siê mocno o jakiœ kod znaleziony na gicie.
Zastanawiam siê, czy to Komando mo¿e uznaæ za plagiat, czy raczej za reu¿ycie kodu? XD 

Socket ma nastêpuj¹ce metody:

1. Server() - otwiera dostêpny na komputerze socket. Serwer ma to do siebie, ¿e ca³y czas s³ucha
2. Receive() - odbiera bajty reprezentuj¹ce przes³an¹ wiadomoœæ i dekoduje na string. Potem coœ mo¿na z tym stringiem zrobiæ, nie wiem
3. Connect() - ³¹czymy siê z jakimœ innym socketem, myœlê ¿e to siê przyda, je¿eli chmura bêdzie musia³a z jakimœ routerem nawi¹zaæ po³¹czenie
4. Send() - przesy³amy strumieñ bajtów reprezentuj¹cy jak¹œ wiadomoœæ
5. MessageProcessing() - no je¿eli w wiadomoœci bêdzie docelowy port pod jaki trzeba przes³aæ pakiet, no to bêdzie trzeba jakoœ to wyci¹gn¹æ



Klasa Time zwraca timestampy w httpd-like formie 
