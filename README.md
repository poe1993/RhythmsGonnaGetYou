# RhythmsGonnaGetYou

CREATE TABLE "Album" (
"Id" SERIAL PRIMARY KEY,
"Title" TEXT NOT NULL,
"IsExplicit" BOOL,
"ReleaseDate" DATE,
);

CREATE TABLE "Bands" (
"Id" SERIAL PRIMARY KEY,
"Name" TEXT NOT NULL,
"CountryOfOrigin" TEXT NOT NULL,
"NumberofMembers" INT,
"Website" TEXT,
"Style" Text,
"IsSigned" BOOL,
"ContactName" TEXT,
"ContactPhoneNumber" TEXT
);

INSERT INTO "Album" ("Title", "IsExplicit", "ReleaseDate")  
 VALUES('Near Dark', False,'2014-08-23');

INSERT INTO "Bands" ("Name", "CountryOfOrigin","NumberofMembers", "Website", "Style", "IsSigned", "ContactName", "ContactPhoneNum
ber")
VALUES('Dance With the Dead' ,'USA', 2, 'https://dancewiththedead.bandcamp.com'
,'Synthwave', False, 'Eric Poe', '813-943-8123');

ALTER TABLE "Album" ADD COLUMN "BandId" INTEGER NULL REFERENCES "Bands"("Id");

UPDATE "Album"
SET "BandId" = 1
WHERE "Title" = 'Near Dark';
