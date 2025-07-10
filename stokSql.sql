CREATE DATABASE stok_takip;
USE stok_takip;

CREATE TABLE tedarikciler (
    id INT AUTO_INCREMENT PRIMARY KEY,
    tedarikci_ad VARCHAR(50),
    tedarikci_no VARCHAR(20),  
    tedarikci_adres TEXT,
    tedarikci_email VARCHAR(60)
);

CREATE TABLE urunler (
    id INT AUTO_INCREMENT PRIMARY KEY,
    urun_ad VARCHAR(60) NOT NULL,
    urun_fiyat DECIMAL(10,2),
    urun_STT DATE,
    alarm_seviyesi INT,
    kategori VARCHAR(25),
    tedarikci INT,
    FOREIGN KEY (tedarikci) REFERENCES tedarikciler(id)
);

CREATE TABLE stok (
    id INT AUTO_INCREMENT PRIMARY KEY,
    stok_adet INT NOT NULL,
    urun INT NOT NULL,
    FOREIGN KEY (urun) REFERENCES urunler(id)
);


