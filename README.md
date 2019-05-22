# ExpressionBuilder

Create expressions programmatially to be used with Linq to Entities or Linq to DocumentDb.

<b>Usage</b>

<pre>var exp = new ExpressionBuilder&lt;Entity&gt;();
exp.Append(c => c.PropertyOne == 1);
exp.Append(c => c.PropertyTwo == true);
</pre>

Sources:
https://blogs.msdn.microsoft.com/meek/2008/05/02/linq-to-entities-combining-predicates/  
