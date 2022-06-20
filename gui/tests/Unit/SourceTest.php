<?php

namespace Tests\Unit;

use App\Models\Source;
use InvalidArgumentException;
use PHPUnit\Framework\TestCase;

class SourceTest extends TestCase
{
    /**
     * Tests if the ID is reliably parsed from a given long URL (https://www.youtube.com/watch?v={id}).
     *
     * @return void
     */
    public function test_id_is_parsed_from_long_url()
    {
        $expected = 'dd7BILZcYAY';
        $actual   = Source::parse('https://www.youtube.com/watch?v=dd7BILZcYAY');

        $this->assertEquals($expected, $actual);
    }

    /**
     * Tests if the ID is reliably parsed from a given short URL (https://youtu.be/{id}).
     *
     * @return void
     */
    public function test_id_is_parsed_from_short_url()
    {
        $expected = 'dd7BILZcYAY';
        $actual   = Source::parse('https://youtu.be/dd7BILZcYAY');

        $this->assertEquals($expected, $actual);
    }

    /**
     * Tests if the ID is reliably parsed from a given short URL (https://youtu.be/{id}).
     *
     * @return void
     */
    public function test_invalid_url_throws_exception()
    {
        $this->expectException(InvalidArgumentException::class);
        Source::parse('dd7BILZcYAYno');
    }
}
