<?php

namespace Tests\Unit;

use App\Models\Identifier;
use PHPUnit\Framework\TestCase;

class IdentifierTest extends TestCase
{
    /**
     * Tests if the correct ID is inferred for a given client.
     *
     * @return void
     */
    public function test_identifier_is_correctly_generated()
    {
        $expected = '12ca17b49af2289436f303e0166030a21e525d266e209267433801a8fd4071a0';
        $actual   = Identifier::infer('127.0.0.1');

        $this->assertEquals($expected, $actual);
    }
}
