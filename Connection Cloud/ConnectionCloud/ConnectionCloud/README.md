To jest szkielet serwera UDP. A w�a�ciwie nie serwera, a socketu. Zrobi�em to opieraj�c si� mocno o jaki� kod znaleziony na gicie.
Zastanawiam si�, czy to Komando mo�e uzna� za plagiat, czy raczej za reu�ycie kodu? XD 

Socket ma nast�puj�ce metody:

1. Server() - otwiera dost�pny na komputerze socket. Serwer ma to do siebie, �e ca�y czas s�ucha
2. Receive() - odbiera bajty reprezentuj�ce przes�an� wiadomo�� i dekoduje na string. Potem co� mo�na z tym stringiem zrobi�, nie wiem
3. Connect() - ��czymy si� z jakim� innym socketem, my�l� �e to si� przyda, je�eli chmura b�dzie musia�a z jakim� routerem nawi�za� po��czenie
4. Send() - przesy�amy strumie� bajt�w reprezentuj�cy jak�� wiadomo��
5. MessageProcessing() - no je�eli w wiadomo�ci b�dzie docelowy port pod jaki trzeba przes�a� pakiet, no to b�dzie trzeba jako� to wyci�gn��



Klasa Time zwraca timestampy w httpd-like formie 
