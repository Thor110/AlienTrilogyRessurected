Exception encountered, of type &quot;Error&quot;<br />
[ZGJSyd95gOa2IDzjYWV15AAAAF4] /load.php?debug=false&amp;lang=en&amp;modules=startup&amp;only=scripts&amp;skin=vector   Error from line 663 of /home/u6868p4663/domains/wiki.xentax.com/public_html/includes/exception/MWExceptionHandler.php: Class 'FormatJson' not found<br />
Backtrace:<br />
#0 /home/u6868p4663/domains/wiki.xentax.com/public_html/includes/exception/MWExceptionHandler.php(242): MWExceptionHandler::logError(ErrorException, string)<br />
#1 /home/u6868p4663/domains/wiki.xentax.com/public_html/includes/AutoLoader.php(81): MWExceptionHandler::handleError(integer, string, string, integer, array)<br />
#2 /home/u6868p4663/domains/wiki.xentax.com/public_html/includes/AutoLoader.php(81): require()<br />
#3 [internal function]: AutoLoader::autoload(string)<br />
#4 /home/u6868p4663/domains/wiki.xentax.com/public_html/includes/resourceloader/ResourceLoader.php(130): spl_autoload_call(string)<br />
#5 /home/u6868p4663/domains/wiki.xentax.com/public_html/includes/resourceloader/ResourceLoaderStartUpModule.php(397): ResourceLoader-&gt;preloadModuleInfo(array, DerivativeResourceLoaderContext)<br />
#6 /home/u6868p4663/domains/wiki.xentax.com/public_html/includes/resourceloader/ResourceLoaderStartUpModule.php(379): ResourceLoaderStartUpModule-&gt;getAllModuleHashes(DerivativeResourceLoaderContext)<br />
#7 /home/u6868p4663/domains/wiki.xentax.com/public_html/includes/resourceloader/ResourceLoaderModule.php(707): ResourceLoaderStartUpModule-&gt;getDefinitionSummary(DerivativeResourceLoaderContext)<br />
#8 /home/u6868p4663/domains/wiki.xentax.com/public_html/includes/resourceloader/ResourceLoader.php(622): ResourceLoaderModule-&gt;getVersionHash(DerivativeResourceLoaderContext)<br />
#9 [internal function]: ResourceLoader-&gt;{closure}(string)<br />
#10 /home/u6868p4663/domains/wiki.xentax.com/public_html/includes/resourceloader/ResourceLoader.php(623): array_map(Closure, array)<br />
#11 /home/u6868p4663/domains/wiki.xentax.com/public_html/includes/resourceloader/ResourceLoader.php(675): ResourceLoader-&gt;getCombinedVersion(ResourceLoaderContext, array)<br />
#12 /home/u6868p4663/domains/wiki.xentax.com/public_html/load.php(47): ResourceLoader-&gt;respond(ResourceLoaderContext)<br />
#13 {main}<br />

