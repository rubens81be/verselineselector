;WITH XMLNAMESPACES('urn:be:fgov:ehealth:samws:v2:export' AS ns2,
                    DEFAULT 'urn:be:fgov:ehealth:samws:v2:chapteriv:submit')
INSERT INTO [dbo].[SAMV2_VERSE](
  [CHAPTER],[PARAGRAPH],[SEQUENCE],[VALIDFROM],[VALIDTO],[NUMBER],[PARENT],[LEVEL],[TYPE],[CHECKBOX],[MINCHECK],[ANDCLAUSENR],
  [TEXT_NL],[REQUESTTYPE],[AGREEMENTTERM],[AGREEMENTTERMUNIT],[SEXRESTRICTED],[MINAGEAUTHORIZED],[MINAGEAUTHUNIT],[MAXAGEAUTHORIZED],[MAXAGEAUTHUNIT],[OTHERADDEDDOC])
SELECT 
  c.value('../../@ChapterName[1]', 'nvarchar(50)') ChapterName,
  c.value('../../@ParagraphName[1]', 'nvarchar(50)') ParagraphName,
  c.value('../@VerseSeq[1]', 'int') SequenceNumber,
  c.value('@from[1]', 'date') ValidFrom,
  c.value('@to[1]', 'date') ValidTo,
  c.value('VerseNum[1]', 'int') VerseNumber,
  c.value('VerseSeqParent[1]', 'int') VerseSeqParent,
  c.value('VerseLevel[1]', 'int') VerseLevel,
  c.value('VerseType[1]', 'nvarchar(2)') VerseType,
  ISNULL(c.value('CheckBoxInd[1]', 'bit'), 0) CheckBoxInd,
  ISNULL(c.value('MinCheckNum[1]', 'int'), 0) MinCheckNum,
  ISNULL(c.value('AndClauseNum[1]', 'int'), 0) AndClauseNum,
  c.value('TextNl[1]', 'nvarchar(max)') TextNl,
  c.value('RequestType[1]', 'nvarchar(1)') RequestType,
  c.value('AgreementTerm[1]', 'int') AgreementTerm,
  c.value('AgreementTermUnit[1]', 'nvarchar(1)') AgreementTermUnit,
  c.value('SexRestricted[1]', 'nvarchar(1)') SexRestricted,
  c.value('MinimumAgeAuthorized[1]', 'int') MinimumAgeAuthorized,
  c.value('MinimumAgeAuthorizedUnit[1]', 'nvarchar(1)') MinimumAgeAuthorizedUnit,
  c.value('MaximumAgeAuthorized[1]', 'int') MaximumAgeAuthorized,
  c.value('MaximumAgeAuthorizedUnit[1]', 'nvarchar(1)') MaximumAgeAuthorizedUnit,
  c.value('OtherAddedDocumentInd[1]', 'bit') CheckBoxInd

FROM [dbo].[SAMV2_IMPORT] t
CROSS APPLY t.ChapterIV.nodes('/ns2:ExportChapterIV/ns2:Paragraph/ns2:Verse/ns2:Data') AS R(c)
WHERE c.value('../../@ParagraphName[1]', 'nvarchar(10)') = '8090000'
AND c.value('@to[1]', 'nvarchar(10)') IS NULL
ORDER BY SequenceNumber