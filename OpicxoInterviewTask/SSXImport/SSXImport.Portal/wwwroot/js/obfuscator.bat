if %2 == 1 echo "Obfuscator Started" && ^
javascript-obfuscator %1wwwroot\js\APIData\ApiData.js --output  %1wwwroot\js\APIData\ApiData.min.js && ^
javascript-obfuscator %1wwwroot\js\APIData\ApiData.InputParameter.js --output  %1wwwroot\js\APIData\ApiData.InputParameter.min.js && ^
javascript-obfuscator %1wwwroot\js\APIData\ApiData.OutputParameter.js --output  %1wwwroot\js\APIData\ApiData.OutputParameter.min.js && ^
javascript-obfuscator %1wwwroot\js\APIData\ApidataList.js --output  %1wwwroot\js\APIData\ApidataList.min.js && ^
javascript-obfuscator %1wwwroot\js\DataTransfer\DataTransferList.js --output  %1wwwroot\js\DataTransfer\DataTransferList.min.js && ^
javascript-obfuscator %1wwwroot\js\Template\Template.js --output  %1wwwroot\js\Template\Template.min.js && ^
javascript-obfuscator %1wwwroot\js\Template\Template.Mapping.js --output  %1wwwroot\js\Template\Template.Mapping.min.js && ^
javascript-obfuscator %1wwwroot\js\Template\Template.Origin.js --output  %1wwwroot\js\Template\Template.Origin.min.js && ^
javascript-obfuscator %1wwwroot\js\Template\Template.Target.js --output  %1wwwroot\js\Template\Template.Target.min.js && ^
javascript-obfuscator %1wwwroot\js\Template\Template.TransferSchedule.js --output  %1wwwroot\js\Template\Template.TransferSchedule.min.js && ^
javascript-obfuscator %1wwwroot\js\Template\TemplateList.js --output  %1wwwroot\js\Template\TemplateList.min.js && ^
javascript-obfuscator %1wwwroot\js\GenericTemplate\GenericTemplateList.js --output  %1wwwroot\js\GenericTemplate\GenericTemplateList.min.js && ^
javascript-obfuscator %1wwwroot\js\GenericTemplate\GenericTemplate.js --output  %1wwwroot\js\GenericTemplate\Genericemplate.min.js && ^
echo "Obfuscator Completed"