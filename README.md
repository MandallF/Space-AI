***Space AI Oyunu

-Bu proje, bir yapay sinir ağının (Neural Network) oyuncu hareketlerini taklit ederek (Imitation Learning) engellerden kaçmayı öğrendiği bir eğitim çalışmasıdır. Proje, temel yapay zeka prensiplerini ve WebGL platformu üzerindeki veri yönetimini sergilemektedir.

***Oyunu tarayıcı üzerinden hemen deneyebilirsiniz: https://mandalf.itch.io/spaceai

<img width="960" height="601" alt="image" src="https://github.com/user-attachments/assets/999c3fb1-6b39-4b65-89c3-b3b374a659bd" />

Linke tıkladığınızda böyle bir ekran gelecek. Burada "Run Game" diyerek oyunu başlatın.

<img width="970" height="649" alt="image" src="https://github.com/user-attachments/assets/31f24849-ae32-4969-9696-97b15ddaae4a" />

Ardından ise sizi oyunumun ana menüsü karşılayacak. Burada en alttaki seçenek oyunun ses efeklerinin şiddetini ayarlıyor, üstündeki seçenek müzik şiddetini ayarlıyor ve en üstteki "New Game" seçeneği yeni oyun için başlangıç seçeneklerini getiriyor.

<img width="771" height="505" alt="image" src="https://github.com/user-attachments/assets/b8a7e8c8-ea3b-4837-9753-67dd69562ca8" />

Burada eğer "Kendim Oyna" derseniz yapay zeka ağırlıklarını kullanmadan ister eğlence isterseniz de yapay zekayı eğitmek için oyunu başlatıyorsunuz. Oyun esnasında yapay zeka sizin oynayışınızı izliyor ve siz de eğer bu oynayışı kaydetsin istiyorsanız
klavyeden "K" tuşuna basarak "ai-weights.json" dosyası oluşturmasını sağlıyorsunuz. Ardından eğer yapay zekanın oynamasını isterseniz bu sefer "New Game" dediğinizde "Yapay Zeka Başlat" seçeneğini seçmeniz gerekiyor. Sizin oynayışınıza göre
eğitilen ve kaydedilen ağırlıklara göre yapay zeka oynamaya başlıyor.

<img width="1014" height="697" alt="image" src="https://github.com/user-attachments/assets/9b45268f-f135-4b0f-a127-c99c32eedddd" />

Oyun sırasında ise sol üstte gördüğünüz yeşil slider bizim canımızı gösteriyor. Eğer siz boss'un oluşturduğu minnionları zamanında laseriniz ile yok edemezseniz veya kaçınamazsanız canınız 10 azalıyor. Ancak merak etmeyin, oyunda bir level sistemi mevcut ve
her level atladığınızda 10 can puanı kazanıyorsunuz. Levelinizi takip etmek için sağ üstteki sarı slider sizin XP durumunuzu ve altındaki yazı da şu anda hangi levelde olduğunuzu gösteriyor. İlk başta level atlamak için 100XP gerekirken level atladıkça bu 
10'arlı şekilde artıyor ancak merak etmeyin level atladıkça sizin karşınıza aşağıdaki gibi bir ekran geliyor ve buradan kendinize güçlendirme seçebiliyorsunuz.

<img width="995" height="673" alt="image" src="https://github.com/user-attachments/assets/8e1df719-77aa-4db0-aa58-01c8eff0562d" />

Burada da dikkat etmeniz gereken şey kendi oynayış tarzınıza göre bir karar vermek olacak. Çünkü siz minionları zamanında temizleyemezseniz bunlar birikecektir ve zamanlı sizin kaçacak bir yeriniz kalmaz ancak çok yavaş hareket ederseniz de bu sefer zamanında
hareket edemeyebilirsiniz.

<img width="1046" height="684" alt="image" src="https://github.com/user-attachments/assets/7354b86d-481e-4774-9239-63e500fcf6db" />

Oyunu kazanmak içinse her 10 levelde bir şansınızı deneyebiliyorsunuz. Burada boss artık sizi kendi minnionları ile yenemeyeceğini anlıyor ve öfkesinden kendi kalkanını kırıyor. Renk değişikliğinden ve müziğin hızlanmasından anlayabilirsiniz. Burada acele
etmelisiniz çünkü 30 saniye sonra boss tekrardan kalkan açıyor. Boss fight sırasında da güçlenme şansınız var bossa her hasar verdiğinizde 1 xp kazanıyorsuz ve bossun da 1000 canı var. Sizin mermileriniz bossun 10 canını götürüyor. Eğer vaktinde kesebilirseniz
kazanırsınız ve alttaki ekranı görürsünüz. Burada isterseniz tekrar oynyabilirsiniz. Kesemezseniz boss canını ve kalkanını yenilese de üzülmeyin her 10 level atladığınızda tekrar deneme şansınız var. Güçlenip geri dönebilirsiniz.

<img width="1002" height="678" alt="image" src="https://github.com/user-attachments/assets/f5eeb633-3a5d-47e0-aa46-995adae34b6f" />

Ayriyeten tekrar oynayarak kendi skorunuzu geçmeye hatta AI oynayışını da deneyerek onunla yarışabilirsiniz. Kazandığınızda "R" tuşuna basarak ana menüye dönebilir ve isterseniz kendiniz, isterseniz de AI oynayışını deneyebilirsiniz.

<img width="997" height="673" alt="image" src="https://github.com/user-attachments/assets/29934e94-f077-4d07-b31d-175eb60e9614" />

Kaybederseniz böyle bir ekran görüyorsunuz ancak üzülmeyin tekrar başlamak için "R" tuşuna basmanız yeterli.

------------------------------------------------------
*Yapay Zeka Mimarisi ve Teknik Detaylar
1. Kullanılan Yöntem
Ajan, Çok Katmanlı Algılayıcı (Multilayer Perceptron) mimarisine sahip bir Yapay Sinir Ağı kullanmaktadır. Eğitim süreci, oyuncunun klavye girdilerini (Input.GetAxisRaw) referans alan İmitasyon Öğrenmesi (Imitation Learning) ile gerçekleştirilir.

Input Layer (Giriş Katmanı): Ajanın konumu, en yakın engelin mesafesi ve yönü.
Hidden Layer (Gizli Katman): Nöronlar arası ağırlıklar (weights) ve aktivasyon fonksiyonları.
Output Layer (Çıkış Katmanı): Sağa, sola hareket veya sabit durma kararı.

2. Kural Tabanlı Olmayan Yapı
Ajanın kararları "if-else" blokları veya önceden yazılmış kural setleri ile değildir. Tüm hareket kararları, nöronlar arasındaki ağırlıkların matris çarpımları sonucunda dinamik olarak hesaplanır.

3. Veri Yönetimi ve Dosya Okuma
Kayıt Sistemi: Eğitim modunda "K" tuşuna basıldığında, o anki başarılı ağırlıklar JSON formatında serileştirilerek kaydedilir.
Dinamik Yükleme: Ana menüden "Yapay Zeka'yı Yükle" seçeneği seçildiğinde, ai_weights.json dosyasındaki veriler parse edilerek sinir ağı katmanlarına aktarılır.
Rastgele Davranış: Eğer herhangi bir ağırlık dosyası yüklenmezse, ajan rastgele atanmış ağırlıklarla (eğitilmemiş modda) başlar ve rasyonel olmayan hareketler sergiler.

***Dosya Yapısı ve Önemli Scriptler
ai_weights.json: Eğitilmiş modelin sinaps ağırlıklarını içeren ana veri dosyası.
AgentController.cs: Ajanın hareket mantığı ve sinir ağı ile etkileşimi.
NeuralNetwork.cs: Sinir ağının matematiksel yapısı, ileri besleme (FeedForward) ve ağırlık yönetimi.
GameData.cs: Sahneler arası (Menü -> Oyun) veri ve AI durumu aktarımı.

***Önemli Not (Platform Kısıtlamaları)
WebGL platformundaki tarayıcı güvenlik kısıtlamaları (Sandbox) nedeniyle, oyunun canlı sürümünde ağırlık verileri Resources klasörü altından TextAsset olarak yüklenmektedir.
Ancak projenin kaynak kodlarında ve GitHub dizininde ağırlık dosyası harici bir varlık olarak (ai-weights.json) bağımsızlığını korumaktadır.

----------------------------------------------------------
-Ad Soyad: Kubilay İnanç
-Öğrenci No: 22360859047
-Ders: Oyun Programlama
